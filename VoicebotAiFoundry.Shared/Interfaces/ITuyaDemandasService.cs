using VoicebotAiFoundry.Domain.Entities;
using System.Threading.Tasks;

namespace VoicebotAiFoundry.Shared.Interfaces
{
    public interface ITuyaDemandasService
    {
        Task<string> ProcessDemandRequest(DemandasRequest request);
    }
}
