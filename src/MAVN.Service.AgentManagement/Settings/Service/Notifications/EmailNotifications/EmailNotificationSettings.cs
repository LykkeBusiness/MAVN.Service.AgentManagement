using JetBrains.Annotations;

namespace MAVN.Service.AgentManagement.Settings.Service.Notifications.EmailNotifications
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class EmailNotificationSettings
    {
        public EmailTemplateSettings AgentRejected { get; set; }
    }
}
