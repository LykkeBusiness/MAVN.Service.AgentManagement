using JetBrains.Annotations;
using MAVN.Service.AgentManagement.Settings.Service.Notifications.EmailNotifications;
using MAVN.Service.AgentManagement.Settings.Service.Notifications.PushNotifications;

namespace MAVN.Service.AgentManagement.Settings.Service.Notifications
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class NotificationsSettings
    {
        public PushNotificationSettings PushNotifications { get; set; }

        public EmailNotificationSettings EmailNotifications { get; set; }
    }
}
