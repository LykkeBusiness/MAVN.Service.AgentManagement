using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Common.MsSql;
using MAVN.Service.AgentManagement.Domain.Entities.Agents;
using MAVN.Service.AgentManagement.Domain.Exceptions;
using MAVN.Service.AgentManagement.Domain.Repositories;
using MAVN.Service.AgentManagement.MsSqlRepositories.Contexts;
using MAVN.Service.AgentManagement.MsSqlRepositories.Entities;

namespace MAVN.Service.AgentManagement.MsSqlRepositories.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly MsSqlContextFactory<DataContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly ILog _log;

        public ImageRepository(
            MsSqlContextFactory<DataContext> contextFactory,
            ILogFactory logFactory,
            IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _log = logFactory.CreateLog(this);
        }

        public async Task UpdateAsync(IReadOnlyList<EncryptedImage> images)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    Guid customerId = images.First().CustomerId;

                    try
                    {
                        context.Images.RemoveRange(context.Images.Where(o => o.CustomerId == customerId));
                        context.Images.AddRange(_mapper.Map<List<ImageEntity>>(images));

                        await context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        _log.Error("An error occurred while updating agent images",
                            context: $"customerId: {customerId}");

                        throw new FailedOperationException("Can not update agent images", exception);
                    }
                }
            }
        }
    }
}
