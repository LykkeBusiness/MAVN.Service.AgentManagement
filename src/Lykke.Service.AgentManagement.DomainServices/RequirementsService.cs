using System.Threading.Tasks;
using Falcon.Numerics;
using Lykke.Service.AgentManagement.Domain.Repositories;
using Lykke.Service.AgentManagement.Domain.Services;

namespace Lykke.Service.AgentManagement.DomainServices
{
    public class RequirementsService : IRequirementsService
    {
        private readonly ITokensRequirementRepository _tokensRequirementRepository;

        public RequirementsService(ITokensRequirementRepository tokensRequirementRepository)
        {
            _tokensRequirementRepository = tokensRequirementRepository;
        }

        public async Task<Money18> GetNumberOfTokensAsync()
        {
            return await _tokensRequirementRepository.GetRequiredAmountAsync();
        }

        public async Task UpdateNumberOfTokensAsync(Money18 amount)
        {
            await _tokensRequirementRepository.UpdateAmountAsync(amount);
        }
    }
}
