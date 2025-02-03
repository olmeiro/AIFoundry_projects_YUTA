using VoicebotAiFoundry.Domain.Entities;
using System.Threading.Tasks;

namespace VoicebotAiFoundry.Application.Interfaces
{
    public interface ITuyaDemandasService
    {
        Task<string> ProcessDemandRequest(ChatRequest request);
    }
}
