using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.AgentManagement.Domain.Entities.Agents;
using MAVN.Service.AgentManagement.Domain.Entities.Registration;
using MAVN.Service.AgentManagement.Domain.Entities.Requirements;

namespace MAVN.Service.AgentManagement.Domain.Services
{
    public interface IAgentService
    {
        Task<Agent> GetByCustomerIdAsync(Guid customerId);

        Task<IReadOnlyCollection<Agent>> GetByCustomerIdsAsync(Guid[] customerIds);

        Task RegisterAsync(RegistrationForm registrationForm);

        Task<CustomerRequirements> ValidateAsync(Guid customerId);
    }
}
