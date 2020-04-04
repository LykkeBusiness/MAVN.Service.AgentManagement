using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.AgentManagement.Settings.Service.Rabbit.Publishers
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
