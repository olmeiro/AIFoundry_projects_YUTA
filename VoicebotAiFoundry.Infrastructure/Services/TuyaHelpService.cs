using VoicebotAiFoundry.Domain.Entities;
using VoicebotAiFoundry.Shared.Interfaces;
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

        public async Task<string> ProcessHelpRequest(TuyaHelpRequest request)
        {
            return await _azureOpenAIService.GenerateResponseAsync(new MessageRequest { Message = request.Query });
        }
    }
}
