using Autofac;
using JetBrains.Annotations;
using Lykke.HttpClientGenerator;
using Lykke.HttpClientGenerator.Infrastructure;
using System;

namespace MAVN.Service.AgentManagement.Client
{
    /// <summary>
    /// Extension for client registration.
    /// </summary>
    [PublicAPI]
    public static class AutofacExtension
    {
        /// <summary>
        /// Registers <see cref="IAgentManagementClient"/> in Autofac container using <see cref="AgentManagementServiceClientSettings"/>.
        /// </summary>
        /// <param name="builder">Autofac container builder.</param>
        /// <param name="settings">Agent management client settings.</param>
        /// <param name="builderConfigure">Optional <see cref="HttpClientGeneratorBuilder"/> configure handler.</param>
        public static void RegisterAgentManagementClient(
            [NotNull] this ContainerBuilder builder,
            [NotNull] AgentManagementServiceClientSettings settings,
            [CanBeNull] Func<HttpClientGeneratorBuilder, HttpClientGeneratorBuilder> builderConfigure = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (string.IsNullOrWhiteSpace(settings.ServiceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.",
                    nameof(AgentManagementServiceClientSettings.ServiceUrl));

            var clientBuilder = HttpClientGenerator.HttpClientGenerator.BuildForUrl(settings.ServiceUrl)
                .WithAdditionalCallsWrapper(new ExceptionHandlerCallsWrapper());

            clientBuilder = builderConfigure?.Invoke(clientBuilder) ?? clientBuilder.WithoutRetries();

            builder.RegisterInstance(new AgentManagementClient(clientBuilder.Create()))
                .As<IAgentManagementClient>()
                .SingleInstance();
        }
    }
}
