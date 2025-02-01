using System.Threading.Tasks;
using VoicebotAiFoundry.Domain.Entities;


namespace VoicebotAiFoundry.Shared.Interfaces
{
    public interface IAzureOpenAIService
    {
        Task<string> GenerateResponseAsync(MessageRequest message);
    }
}
