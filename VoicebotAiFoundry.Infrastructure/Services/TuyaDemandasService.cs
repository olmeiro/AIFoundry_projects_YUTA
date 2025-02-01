using VoicebotAiFoundry.Domain.Entities;
using VoicebotAiFoundry.Application.Interfaces;
using System.Threading.Tasks;

namespace VoicebotAiFoundry.Infrastructure.Services
{
    public class TuyaDemandasService : ITuyaDemandasService
    {
        private readonly IAzureOpenAIService _azureOpenAIService;

        public TuyaDemandasService(IAzureOpenAIService azureOpenAIService)
        {
            _azureOpenAIService = azureOpenAIService;
        }

        public async Task<string> ProcessDemandRequest(Message request)
        {
            return await _azureOpenAIService.GenerateResponseAsync(request);
        }
    }
}
