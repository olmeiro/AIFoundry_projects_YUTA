using VoicebotAiFoundry.Domain.Entities;
using System.Threading.Tasks;

namespace VoicebotAiFoundry.Shared.Interfaces
{
    public interface ITuyaHelpService
    {
        Task<string> ProcessHelpRequest(TuyaHelpRequest request);
    }
}
