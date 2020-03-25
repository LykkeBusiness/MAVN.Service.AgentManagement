using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.AgentManagement.Domain.Entities.Agents;
using Lykke.Service.AgentManagement.Domain.Entities.Registration;
using Lykke.Service.AgentManagement.Domain.Entities.Requirements;

namespace Lykke.Service.AgentManagement.Domain.Services
{
    public interface IAgentService
    {
        Task<Agent> GetByCustomerIdAsync(Guid customerId);

        Task<IReadOnlyCollection<Agent>> GetByCustomerIdsAsync(Guid[] customerIds);

        Task RegisterAsync(RegistrationForm registrationForm);

        Task<CustomerRequirements> ValidateAsync(Guid customerId);
    }
}
