using VoicebotAiFoundry.Domain.Entities;
using VoicebotAiFoundry.Shared.Interfaces;

using System.Threading.Tasks;

namespace VoicebotAiFoundry.Infrastructure.Services
{
    public class VoicebotService : IVoicebotService
    {
        private readonly IAzureOpenAIService _azureOpenAIService;

        public VoicebotService(IAzureOpenAIService azureOpenAIService)
        {
            _azureOpenAIService = azureOpenAIService;
        }

        public async Task<string> GenerateResponseAsync(Message message)
        {
            return await _azureOpenAIService.GenerateResponseAsync(new MessageRequest { Message = message.Content });
        }
    }
}
