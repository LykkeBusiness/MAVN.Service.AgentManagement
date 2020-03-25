using System.Threading.Tasks;
using Falcon.Numerics;

namespace Lykke.Service.AgentManagement.Domain.Services
{
    public interface IRequirementsService
    {
        Task<Money18> GetNumberOfTokensAsync();

        Task UpdateNumberOfTokensAsync(Money18 amount);
    }
}
