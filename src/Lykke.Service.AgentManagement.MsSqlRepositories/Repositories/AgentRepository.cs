using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Common.MsSql;
using Lykke.Service.AgentManagement.Domain.Entities.Agents;
using Lykke.Service.AgentManagement.Domain.Exceptions;
using Lykke.Service.AgentManagement.Domain.Repositories;
using Lykke.Service.AgentManagement.MsSqlRepositories.Contexts;
using Lykke.Service.AgentManagement.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lykke.Service.AgentManagement.MsSqlRepositories.Repositories
{
    public class AgentRepository : IAgentRepository
    {
        private readonly MsSqlContextFactory<DataContext> _contextFactory;
        private readonly ILog _log;
        private readonly IMapper _mapper;

        public AgentRepository(
            MsSqlContextFactory<DataContext> contextFactory,
            ILogFactory logFactory,
            IMapper mapper)
        {
            _contextFactory = contextFactory;
            _log = logFactory.CreateLog(this);
            _mapper = mapper;
        }

        public async Task<Agent> GetByCustomerIdAsync(Guid customerId)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = await context.Agents.FirstOrDefaultAsync(o => o.CustomerId == customerId);

                return _mapper.Map<Agent>(entity);
            }
        }

        public async Task<IReadOnlyCollection<Agent>> GetByCustomerIdsAsync(Guid[] customerIds)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entities = await context.Agents.Where(o => customerIds.Contains(o.CustomerId)).ToListAsync();

                return _mapper.Map<IReadOnlyCollection<Agent>>(entities);
            }
        }

        public async Task InsertAsync(Agent agent, IReadOnlyList<EncryptedImage> images)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Agents.Add(_mapper.Map<AgentEntity>(agent));

                        context.Images.AddRange(_mapper.Map<List<ImageEntity>>(images));

                        await context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        _log.Error("An error occurred while inserting new agent with images",
                            context: $"customerId: {agent.CustomerId}");

                        throw new FailedOperationException("Can not insert agent", exception);
                    }
                }
            }
        }

        public async Task UpdateStatusAsync(Guid customerId, AgentStatus status)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = await context.Agents.FirstOrDefaultAsync(o => o.CustomerId == customerId);

                if (entity == null)
                    return;

                entity.Status = status;

                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateStatusAsync(Guid customerId, string salesforceId, AgentStatus status)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = await context.Agents.FirstOrDefaultAsync(o => o.CustomerId == customerId);

                if (entity == null)
                    return;

                entity.SalesforceId = salesforceId;
                entity.Status = status;

                await context.SaveChangesAsync();
            }
        }
    }
}
