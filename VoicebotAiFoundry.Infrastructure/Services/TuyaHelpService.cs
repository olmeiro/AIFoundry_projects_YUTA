using VoicebotAiFoundry.Domain.Entities;
using VoicebotAiFoundry.Application.Interfaces;
using System.Threading.Tasks;

namespace VoicebotAiFoundry.Infrastructure.Services
{
    public class TuyaHelpService : ITuyaHelpService
    {
        private readonly IAzureOpenAIService _azureOpenAIService;

        public TuyaHelpService(IAzureOpenAIService azureOpenAIService)
        {
            _azureOpenAIService = azureOpenAIService;
        }

        public async Task<string> ProcessHelpRequest(Message request)
        {
            return await _azureOpenAIService.GenerateResponseAsync(request);
        }
    }
}
