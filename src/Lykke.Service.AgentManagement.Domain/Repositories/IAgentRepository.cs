﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.AgentManagement.Domain.Entities.Agents;

namespace Lykke.Service.AgentManagement.Domain.Repositories
{
    public interface IAgentRepository
    {
        Task<Agent> GetByCustomerIdAsync(Guid customerId);

        Task<IReadOnlyCollection<Agent>> GetByCustomerIdsAsync(Guid[] customerIds);

        Task InsertAsync(Agent agent, IReadOnlyList<EncryptedImage> images);

        Task UpdateStatusAsync(Guid customerId, AgentStatus status);

        Task UpdateStatusAsync(Guid customerId, string salesforceId, AgentStatus status);
    }
}
