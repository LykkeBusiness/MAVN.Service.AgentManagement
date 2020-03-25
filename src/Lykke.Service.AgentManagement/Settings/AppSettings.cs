using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.AgentManagement.Settings.Service;
using Lykke.Service.CustomerProfile.Client;
using Lykke.Service.Dictionaries.Client;
using Lykke.Service.MAVNPropertyIntegration.Client;
using Lykke.Service.PrivateBlockchainFacade.Client;

namespace Lykke.Service.AgentManagement.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public AgentManagementSettings AgentManagementService { get; set; }

        public CustomerProfileServiceClientSettings CustomerProfileServiceClient { get; set; }

        public PrivateBlockchainFacadeServiceClientSettings PrivateBlockchainFacadeServiceClient { get; set; }

        public DictionariesServiceClientSettings DictionariesServiceClient { get; set; }

        public MAVNPropertyIntegrationServiceClientSettings TokenPropertyIntegrationServiceClient { get; set; }
    }
}
