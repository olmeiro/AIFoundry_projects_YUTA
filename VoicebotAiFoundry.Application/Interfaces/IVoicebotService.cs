using VoicebotAiFoundry.Domain.Entities;
using System.Threading.Tasks;

namespace VoicebotAiFoundry.Application.Interfaces
{
    public interface IVoicebotService
    {
        Task<string> GenerateResponseAsync(ChatRequest message);
    }
}
