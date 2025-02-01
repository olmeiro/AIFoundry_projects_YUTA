using VoicebotAiFoundry.Shared.Interfaces;
using VoicebotAiFoundry.Shared.Configuration;
using VoicebotAiFoundry.Domain.Entities;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace VoicebotAiFoundry.Infrastructure.Services
{
    public class AzureOpenAIService : IAzureOpenAIService
    {
        private readonly AzureOpenAIClient _client;
        private readonly string _deploymentId;
        private readonly ILogger<AzureOpenAIService> _logger;

        public AzureOpenAIService(ILogger<AzureOpenAIService> logger, IOptions<AzureOpenAIOptions> options)
        {
            _logger = logger;
            var settings = options.Value;

            if (string.IsNullOrEmpty(settings.Endpoint) || string.IsNullOrEmpty(settings.ApiKey) || string.IsNullOrEmpty(settings.DeploymentId))
            {
                throw new ArgumentException("Faltan configuraciones necesarias para Azure OpenAI.");
            }

            _client = new AzureOpenAIClient(new Uri(settings.Endpoint), new AzureKeyCredential(settings.ApiKey));
            _deploymentId = settings.DeploymentId;
        }

        public async Task<string> GenerateResponseAsync(MessageRequest message)
        {
            _logger.LogInformation("Generando respuesta para: {Message}", message);

            var chatHistory = new List<ChatMessage>
            {
                ChatMessage.CreateSystemMessage("Eres un asistente util"),
                ChatMessage.CreateUserMessage(message.Message)
            };

            var chatRequestOptions = new ChatCompletionOptions()
            {
                Temperature = 0.3f,
                MaxOutputTokenCount = 500,
                TopP = 0.9f,
                FrequencyPenalty = 0.0f,
                PresencePenalty = 0.0f
            };

            try
            {
                ChatClient chatClient = _client.GetChatClient(_deploymentId);

                var response = await chatClient.CompleteChatAsync(chatHistory, chatRequestOptions);

                if (response?.Value?.Content == null || response.Value.Content.Count == 0)

                {
                    _logger.LogWarning("Respuesta vacía de OpenAI.");
                    return "No se obtuvo respuesta del servicio.";
                }

                return response.Value.Content[0].Text;


            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, "Error en la solicitud a Azure OpenAI.");
                return "Hubo un error procesando la solicitud.";
            }
        }
    }
}
