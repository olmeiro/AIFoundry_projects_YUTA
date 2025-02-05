using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VoicebotAiFoundry.Infrastructure.Agents
{
    public class VoicebotPlugin
    {
        [KernelFunction]
        public async Task<string> ProcessVoicebotRequestAsync(
            string input,
            Kernel kernel,
            KernelArguments arguments)
        {
            if (!kernel.Plugins.TryGetPlugin("Voicebot", out var plugin))
            {
                return "No se encontró el plugin de Voicebot.";
            }

            if (!plugin.TryGetFunction("skprompt", out var function))
            {
                return "No se encontró el prompt de Voicebot.";
            }

            return await function.InvokeAsync<string>(kernel, new() { ["input"] = input });
        }
    }
}
