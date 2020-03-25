using JetBrains.Annotations;
using Lykke.Service.AgentManagement.Settings.Service.Notifications.EmailNotifications;
using Lykke.Service.AgentManagement.Settings.Service.Notifications.PushNotifications;

namespace Lykke.Service.AgentManagement.Settings.Service.Notifications
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class NotificationsSettings
    {
        public PushNotificationSettings PushNotifications { get; set; }

        public EmailNotificationSettings EmailNotifications { get; set; }
    }
}
