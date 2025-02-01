using VoicebotAiFoundry.Domain.Entities;
using System.Threading.Tasks;

namespace VoicebotAiFoundry.Application.Interfaces
{
    public interface ITuyaHelpService
    {
        Task<string> ProcessHelpRequest(Message request);
    }
}
