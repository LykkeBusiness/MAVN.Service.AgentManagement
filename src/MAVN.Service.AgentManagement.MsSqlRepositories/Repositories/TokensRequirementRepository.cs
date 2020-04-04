using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Falcon.Numerics;
using Lykke.Common.Log;
using Lykke.Common.MsSql;
using MAVN.Service.AgentManagement.Domain.Repositories;
using MAVN.Service.AgentManagement.MsSqlRepositories.Contexts;
using MAVN.Service.AgentManagement.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.AgentManagement.MsSqlRepositories.Repositories
{
    public class TokensRequirementRepository : ITokensRequirementRepository
    {
        private const int InitialTokenAmount = 100;

        private readonly MsSqlContextFactory<DataContext> _contextFactory;
        private readonly ILog _log;
        private readonly IMapper _mapper;

        public TokensRequirementRepository(
            MsSqlContextFactory<DataContext> contextFactory,
            ILogFactory logFactory,
            IMapper mapper)
        {
            _contextFactory = contextFactory;
            _log = logFactory.CreateLog(this);
            _mapper = mapper;
        }

        public async Task<Money18> GetRequiredAmountAsync()
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = await context.TokensRequirement.FirstOrDefaultAsync();

                if (entity == null)
                {
                    await context.TokensRequirement.AddAsync(new TokensRequirementEntity
                    {
                        Amount = Money18.Create(InitialTokenAmount)
                    });
                    await context.SaveChangesAsync();

                    return InitialTokenAmount;
                }

                return entity.Amount;
            }
        }

        public async Task UpdateAmountAsync(Money18 amount)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = await context.TokensRequirement.FirstOrDefaultAsync();

                if (entity == null)
                {
                    entity = new TokensRequirementEntity { Amount = amount };
                    await context.AddRangeAsync(entity);
                    await context.SaveChangesAsync();
                    return;
                }

                entity.Amount = amount;

                await context.SaveChangesAsync();
            }
        }
    }
}
