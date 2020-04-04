using Autofac;
using Lykke.Common.MsSql;
using MAVN.Service.AgentManagement.Domain.Repositories;
using MAVN.Service.AgentManagement.MsSqlRepositories.Contexts;
using MAVN.Service.AgentManagement.MsSqlRepositories.Repositories;

namespace MAVN.Service.AgentManagement.MsSqlRepositories
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
