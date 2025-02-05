using VoicebotAiFoundry.Domain.Entities;
using VoicebotAiFoundry.Application.Interfaces;

using System.Threading.Tasks;

namespace VoicebotAiFoundry.Infrastructure.Services
{
    public class VoicebotService : IVoicebotService
    {
        private readonly ISemanticKernelService _semanticKernelService;

        public VoicebotService(
            ISemanticKernelService semanticKernelService
        )
        {
            _semanticKernelService = semanticKernelService;
        }

        public async Task<string> GenerateResponseAsync(ChatRequest message)
        {
            return await _semanticKernelService.ExecuteAgentAsync(message);
        }
    }
}
