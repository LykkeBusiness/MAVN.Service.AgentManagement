using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.AgentManagement.Settings.Service.Rabbit.Subscribers
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SubscriberSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }

        public string Exchange { get; set; }

        public string QueueSuffix { get; set; }
    }
}
