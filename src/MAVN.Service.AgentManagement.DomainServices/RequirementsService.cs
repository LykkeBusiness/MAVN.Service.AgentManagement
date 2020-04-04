using System.Threading.Tasks;
using Falcon.Numerics;
using MAVN.Service.AgentManagement.Domain.Repositories;
using MAVN.Service.AgentManagement.Domain.Services;

namespace MAVN.Service.AgentManagement.DomainServices
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
