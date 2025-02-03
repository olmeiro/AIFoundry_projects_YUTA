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

        public async Task<string> ProcessDemandRequest(ChatRequest request)
        {

            var chatRequest = new ChatRequest
            (
                Message: request.Message,  // Asigna el texto del mensaje
                channel: request.Channel,       // Especifica el canal como "VOICE"
                Skill : request.Skill,
                documentNumber: request.DocumentNumber,   // Opcional, si tienes un número de documento
                documentType: request.DocumentType      // Opcional, si tienes un tipo de documento
            );

            return await _azureOpenAIService.GenerateResponseAsync(chatRequest);
        }
    }
}
