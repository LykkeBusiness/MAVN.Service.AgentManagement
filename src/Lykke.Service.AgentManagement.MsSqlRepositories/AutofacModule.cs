using Autofac;
using Lykke.Common.MsSql;
using Lykke.Service.AgentManagement.Domain.Repositories;
using Lykke.Service.AgentManagement.MsSqlRepositories.Contexts;
using Lykke.Service.AgentManagement.MsSqlRepositories.Repositories;

namespace Lykke.Service.AgentManagement.MsSqlRepositories
{
    public class AutofacModule : Module
    {
        private readonly string _connectionString;

        public AutofacModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMsSql(
                _connectionString,
                connString => new DataContext(connString, false),
                dbConn => new DataContext(dbConn));

            builder.RegisterType<AgentRepository>()
                .As<IAgentRepository>()
                .SingleInstance();

            builder.RegisterType<ImageRepository>()
                .As<IImageRepository>()
                .SingleInstance();

            builder.RegisterType<TokensRequirementRepository>()
                .As<ITokensRequirementRepository>()
                .SingleInstance();
        }
    }
}
