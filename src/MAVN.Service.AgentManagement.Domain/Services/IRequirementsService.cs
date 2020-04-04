using System.Threading.Tasks;
using Falcon.Numerics;

namespace MAVN.Service.AgentManagement.Domain.Services
{
    public interface IRequirementsService
    {
        Task<Money18> GetNumberOfTokensAsync();

        Task UpdateNumberOfTokensAsync(Money18 amount);
    }
}
