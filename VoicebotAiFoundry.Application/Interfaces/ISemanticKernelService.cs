using System.Threading.Tasks;
using VoicebotAiFoundry.Domain.Entities;

namespace VoicebotAiFoundry.Application.Interfaces
{
    public interface ISemanticKernelService
    {
        Task<string> ExecuteAgentAsync(ChatRequest request);
    }
}
