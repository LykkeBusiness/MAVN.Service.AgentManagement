using JetBrains.Annotations;
using Lykke.Service.AgentManagement.Settings.Service.Db;
using Lykke.Service.AgentManagement.Settings.Service.Notifications;
using Lykke.Service.AgentManagement.Settings.Service.Rabbit;

namespace Lykke.Service.AgentManagement.Settings.Service
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AgentManagementSettings
    {
        public DbSettings Db { get; set; }

        public string TokenSymbol { get; set; }

        public string NumberOfRequiredTokens { get; set; }

        public string DemoEmailSuffix { get; set; }

        public NotificationsSettings Notifications { get; set; }

        public RabbitSettings Rabbit { get; set; }
    }
}
