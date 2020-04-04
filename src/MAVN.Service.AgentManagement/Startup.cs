using System;
using System.Text.RegularExpressions;
using AutoMapper;
using JetBrains.Annotations;
using Lykke.Logs.Loggers.LykkeSanitizing;
using Lykke.Sdk;
using MAVN.Service.AgentManagement.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MAVN.Service.AgentManagement
{
    [UsedImplicitly]
    public class Startup
    {
        private readonly LykkeSwaggerOptions _swaggerOptions = new LykkeSwaggerOptions
        {
            ApiTitle = "AgentManagement API", ApiVersion = "v1"
        };

        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildServiceProvider<AppSettings>(options =>
            {
                options.SwaggerOptions = _swaggerOptions;

                options.Logs = logs =>
                {
                    logs.AzureTableName = "AgentManagementLog";
                    logs.AzureTableConnectionStringResolver =
                        settings => settings.AgentManagementService.Db.LogsConnectionString;
                    logs.Extended = x =>
                        x.AddSanitizingFilter(new Regex(@"(\\?""?[Pp]hone[Nn]umber\\?""?:\s*\\?"")(.*?)(\\?"")"), "$1*$3")
                        .AddSanitizingFilter(new Regex(@"(\\?""?[Cc]ontent\\?""?:\s*\\?"")(.*?)(\\?"")"), "$1*$3");
                };

                options.Extend = (serviceCollection, settings) =>
                {
                    serviceCollection.AddAutoMapper(typeof(AutoMapperProfile),
                        typeof(MsSqlRepositories.AutoMapperProfile));
                };
            });
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfigurationProvider mapper)
        {
            mapper.AssertConfigurationIsValid();

            app.UseLykkeConfiguration(options => { options.SwaggerOptions = _swaggerOptions; });
        }
    }
}
