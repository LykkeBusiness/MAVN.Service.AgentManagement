using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Falcon.Numerics;
using MAVN.Service.AgentManagement.Domain.Entities.Agents;

namespace MAVN.Service.AgentManagement.Domain.Repositories
{
    public interface ITokensRequirementRepository
    {
        Task<Money18> GetRequiredAmountAsync();

        Task UpdateAmountAsync(Money18 amount);
    }
}
