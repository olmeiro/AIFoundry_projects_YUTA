using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using VoicebotAiFoundry.Domain.Entities;
using VoicebotAiFoundry.Application.Interfaces;
using VoicebotAiFoundry.Shared.Configuration;
using System.Text;
using Microsoft.SemanticKernel.Plugins.Core;

#pragma warning disable SKEXP0050 

namespace VoicebotAiFoundry.Infrastructure.Services
{
    public class SemanticKernelService : ISemanticKernelService
    {
        private readonly Kernel _kernel;
        private readonly ILogger<SemanticKernelService> _logger;

        private readonly KernelPlugin _prompts;
        private readonly KernelPlugin _promptsLanguage;

        public SemanticKernelService(
            IOptions<AzureOpenAIOptions> options,
            ILogger<SemanticKernelService> logger,
            IConfiguration configuration
            )
        {

            _logger = logger;
            var settings = options.Value;
            var builder = Kernel.CreateBuilder();


            builder.AddAzureOpenAIChatCompletion(
                deploymentName: settings.DeploymentId,
                endpoint: settings.Endpoint,
                apiKey: settings.ApiKey,
                modelId: settings.DeploymentId
            );

            _kernel = builder.Build();

            // Cargar Plugins y Prompts
            _kernel.ImportPluginFromType<CurrencyConverter>();
            _kernel.ImportPluginFromType<ConversationSummaryPlugin>();

            string promptsDirectory = configuration.GetValue<string>("SemanticKernel:PromptsPath") 
                         ?? Path.Combine(Directory.GetCurrentDirectory(), "..", "VoicebotAiFoundry.Infrastructure", "Prompts");

            _prompts = _kernel.ImportPluginFromPromptDirectory(promptsDirectory);
            _promptsLanguage = _kernel.ImportPluginFromPromptDirectory(Path.Combine(promptsDirectory, "LanguageHelper"));

            _logger.LogInformation("✅ Semantic Kernel inicializado correctamente.");

        }


        public async Task<string> ExecuteAgentAsync(ChatRequest request)
        {
            _logger.LogInformation("🔎 Procesando mensaje: {Message}", request.Message);

            // Obtener intención del usuario
            var intent = await GetIntent(request.Message);

            switch (intent)
            {
                case "ConvertCurrency":
                    return await HandleCurrencyConversion(request);
                case "SuggestDestinations":
                    return await HandleDestinationSuggestions(request);
                case "SuggestActivities":
                    return await HandleActivitySuggestions(request);
                case "HelpfulPhrases":
                    return await HandleHelpfulPhrases(request);
                case "Translate":
                    return await HandleTranslation(request);
                default:
                    return await HandleDefaultResponse(request);
            }
        }

        private async Task<string> GetIntent(string message)
        {
            _logger.LogInformation("🤖 Analizando intención del mensaje...");

            var intent = await _kernel.InvokeAsync<string>(
                _prompts["GetIntent"],
                new() { { "input", message } }
            );

            _logger.LogInformation("🎯 Intención detectada: {Intent}", intent);
            return intent;
        }

        private async Task<string> HandleCurrencyConversion(ChatRequest request)
        {
            _logger.LogInformation("💱 Procesando conversión de moneda...");

            var currencyText = await _kernel.InvokeAsync<string>(
                _prompts["GetTargetCurrencies"],
                new() { { "input", request.Message } }
            );

            var currencyInfo = currencyText?.Split("|");
            if (currencyInfo == null || currencyInfo.Length < 3)
                return "⚠️ No se pudo identificar la conversión solicitada.";

            var result = await _kernel.InvokeAsync<string>(
                "CurrencyConverter",
                "ConvertAmount",
                new() {
                    { "targetCurrencyCode", currencyInfo[0] },
                    { "baseCurrencyCode", currencyInfo[1] },
                    { "amount", currencyInfo[2] },
                }
            );

            return $"💵 Conversión: {result}";
        }

        private async Task<string> HandleDestinationSuggestions(ChatRequest request)
        {
            try
            {
                _logger.LogInformation("🏝️ Buscando recomendaciones de destinos...");
                FunctionResult result = await _kernel.InvokePromptAsync(request.Message);

                string response = result.GetValue<string>() ?? "No se pudo obtener una recomendación.";

                _logger.LogInformation("🌍 Respuesta generada: {Response}", response);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al obtener recomendaciones de destino: {Error}", ex.Message);
                return "Ocurrió un error al procesar la solicitud.";
            }
        }

        private async Task<string> HandleActivitySuggestions(ChatRequest request)
        {
            try
            {

                _logger.LogInformation("🎭 Sugiriendo actividades...");

                var chatSummary = await _kernel.InvokeAsync<string>(
                    "ConversationSummaryPlugin",
                    "SummarizeConversation",
                    new() { { "input", request.Message } });

                var activities = await _kernel.InvokePromptAsync(
                    request.Message,
                    new() {
                    { "history", chatSummary },
                    { "ToolCallBehavior", ToolCallBehavior.AutoInvokeKernelFunctions }
                    });

                string response = activities.GetValue<string>() ?? "No se pudo obtener una sugerencias de actividades.";
                _logger.LogInformation("🎡 Actividades sugeridas: {Response}", response);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al obtener sugerencias de actividades en el destino: {Error}", ex.Message);
                return "Ocurrió un error al procesar la solicitud.";
            }
        }

        private async Task<string> HandleHelpfulPhrases(ChatRequest request)
        {
            try
            {
                _logger.LogInformation("📖 Buscando frases útiles...");

                string? phrases = await _kernel.InvokeAsync<string>(
                    _promptsLanguage["GetHelpfulPhrases"],
                    new() { { "language", request.Message } }
                );

                _logger.LogInformation("✅ Frases obtenidas: {Phrases}", phrases);

                return phrases ?? "No se encontraron frases útiles.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al obtener frases útiles: {Error}", ex.Message);
                return "Ocurrió un error al procesar la solicitud.";
            }
        }

        private async Task<string> HandleTranslation(ChatRequest request)
        {
            _logger.LogInformation("🌎 Traduciendo mensaje...");
            var response = await _kernel.InvokePromptAsync(request.Message, new()
            {
                { "ToolCallBehavior", ToolCallBehavior.AutoInvokeKernelFunctions }
            });

            return response.GetValue<string>() ?? "No se pudo realizar la traducción";
        }

        private async Task<string> HandleDefaultResponse(ChatRequest request)
        {
            _logger.LogInformation("🤷 Respuesta por defecto...");
            var response = await _kernel.InvokePromptAsync(request.Message, new()
            {
                { "ToolCallBehavior", ToolCallBehavior.AutoInvokeKernelFunctions }
            });

            return response.GetValue<string>() ?? "No se pudo realizar la respuesta por defecto";
        }
    }
}
