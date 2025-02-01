using VoicebotAiFoundry.Domain.Entities;
using VoicebotAiFoundry.Shared.Interfaces;
using System.Threading.Tasks;

namespace VoicebotAiFoundry.Application.UseCases
{
    public class MessageService : IMessageService
    {
        private readonly IAzureOpenAIService _azureOpenAIService;

        public MessageService(IAzureOpenAIService azureOpenAIService)
        {
            _azureOpenAIService = azureOpenAIService;
        }

        public async Task<string> GenerateResponseAsync(Message message)
        {
            return await _azureOpenAIService.GenerateResponseAsync(new MessageRequest { Message = message.Content });
        }
    }
}
