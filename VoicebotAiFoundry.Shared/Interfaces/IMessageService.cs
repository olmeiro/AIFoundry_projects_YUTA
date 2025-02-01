using VoicebotAiFoundry.Domain.Entities;
using System.Threading.Tasks;

namespace VoicebotAiFoundry.Shared.Interfaces
{
    public interface IMessageService
    {
        Task<string> GenerateResponseAsync(Message message);
    }
}
