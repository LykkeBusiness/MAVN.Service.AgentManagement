using JetBrains.Annotations;

namespace Lykke.Service.AgentManagement.Settings.Service.Notifications.EmailNotifications
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class EmailNotificationSettings
    {
        public EmailTemplateSettings AgentRejected { get; set; }
    }
}
