using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace VoicebotAiFoundry.Infrastructure.Agents
{
    public class TuyaHelpPlugin
    {
        [KernelFunction]
        public async Task<string> ProcessTuyaHelpRequestAsync(
            string input,
            Kernel kernel,
            OpenAIPromptExecutionSettings? executionSettings = null)
        {
            var prompt = @"Eres un bot especializado en atender soporte de clientes por WhatsApp.
                         Tu respuesta debe ser breve, educada y efectiva.

                         Consulta: {{$input}}";

            var function = kernel.CreateFunctionFromPrompt(
                prompt,
                executionSettings: executionSettings);

            return await function.InvokeAsync<string>(kernel, new() { ["input"] = input });
        }
    }
}