using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace VoicebotAiFoundry.Infrastructure.Agents
{
    public class DemandasPlugin
    {
        [KernelFunction]
        public async Task<string> ProcessDemandasRequestAsync(
            string input,
            Kernel kernel,
            OpenAIPromptExecutionSettings? executionSettings = null)
        {
            var prompt = @"Eres un asistente legal especializado en demandas judiciales.
                         Responde con términos formales y basados en la normativa vigente.

                         Consulta: {{$input}}";

            var function = kernel.CreateFunctionFromPrompt(
                prompt,
                executionSettings: executionSettings);

            return await function.InvokeAsync<string>(kernel, new() { ["input"] = input });
        }
    }
}