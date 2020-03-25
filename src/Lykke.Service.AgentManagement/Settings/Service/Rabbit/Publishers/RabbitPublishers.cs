using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.AgentManagement.Settings.Service.Rabbit.Publishers
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RabbitPublishers
    {
        [AmqpCheck]
        public string PushNotificationsConnString { get; set; }

        [AmqpCheck]
        public string EmailNotificationsConnString { get; set; }
    }
}
