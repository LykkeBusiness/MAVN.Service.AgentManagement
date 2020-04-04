using Autofac;
using JetBrains.Annotations;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Sdk;
using MAVN.Service.AgentManagement.Services;
using MAVN.Service.AgentManagement.Settings;
using Lykke.Service.CustomerProfile.Client;
using Lykke.Service.Dictionaries.Client;
using Lykke.Service.MAVNPropertyIntegration.Client;
using Lykke.Service.NotificationSystem.SubscriberContract;
using Lykke.Service.PrivateBlockchainFacade.Client;
using Lykke.SettingsReader;

namespace MAVN.Service.AgentManagement
{
    [UsedImplicitly]
    public class AutofacModule : Module
    {
        private const string PushNotifiationsExchangeName = "lykke.notificationsystem.command.pushnotification";
        private const string EmailsExchangeName = "lykke.notificationsystem.command.emailmessage";

        private readonly AppSettings _settings;

        public AutofacModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings.CurrentValue;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>()
                .SingleInstance()
                .AutoActivate();

            builder.RegisterModule(
                new DomainServices.AutofacModule(
                    _settings.AgentManagementService.TokenSymbol,
                    _settings.AgentManagementService.DemoEmailSuffix,
                    _settings.AgentManagementService.Notifications.PushNotifications.AgentApprovedTemplateId,
                    _settings.AgentManagementService.Notifications.PushNotifications.AgentRejectedTemplateId,
                    _settings.AgentManagementService.Notifications.EmailNotifications.AgentRejected.SubjectTemplateId,
                    _settings.AgentManagementService.Notifications.EmailNotifications.AgentRejected.MessageTemplateId));
            builder.RegisterModule(
                new MsSqlRepositories.AutofacModule(_settings.AgentManagementService.Db.DataConnectionString));

            RegisterClients(builder);
            RegisterRabbit(builder);
        }

        private void RegisterClients(ContainerBuilder builder)
        {
            builder.RegisterCustomerProfileClient(_settings.CustomerProfileServiceClient);
            builder.RegisterPrivateBlockchainFacadeClient(_settings.PrivateBlockchainFacadeServiceClient, null);
            builder.RegisterDictionariesClient(_settings.DictionariesServiceClient);
            builder.RegisterMAVNPropertyIntegrationClient(_settings.TokenPropertyIntegrationServiceClient, null);
        }

        private void RegisterRabbit(ContainerBuilder builder)
        {
            builder.RegisterJsonRabbitPublisher<PushNotificationEvent>(
                _settings.AgentManagementService.Rabbit.Publishers.PushNotificationsConnString,
                PushNotifiationsExchangeName);

            builder.RegisterJsonRabbitPublisher<EmailMessageEvent>(
                _settings.AgentManagementService.Rabbit.Publishers.EmailNotificationsConnString,
                EmailsExchangeName);
        }
    }
}
