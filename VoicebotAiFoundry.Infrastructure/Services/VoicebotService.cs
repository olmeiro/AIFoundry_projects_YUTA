using VoicebotAiFoundry.Domain.Entities;
using VoicebotAiFoundry.Application.Interfaces;

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

        public async Task<string> GenerateResponseAsync(ChatRequest message)
        {

            var chatRequest = new ChatRequest
            (
                Message: message.Message,  // Asigna el texto del mensaje
                channel: message.Channel,       // Especifica el canal como "VOICE"
                Skill : message.Skill,
                documentNumber: message.DocumentNumber,   // Opcional, si tienes un número de documento
                documentType: message.DocumentType      // Opcional, si tienes un tipo de documento
            );

            return await _azureOpenAIService.GenerateResponseAsync(chatRequest);
        }
    }
}
