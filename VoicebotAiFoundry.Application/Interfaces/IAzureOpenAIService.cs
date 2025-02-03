using System.Threading.Tasks;
using VoicebotAiFoundry.Domain.Entities;


namespace VoicebotAiFoundry.Application.Interfaces
{
    public interface IAzureOpenAIService
    {
        Task<string> GenerateResponseAsync(ChatRequest chatRequest);
    }
}
