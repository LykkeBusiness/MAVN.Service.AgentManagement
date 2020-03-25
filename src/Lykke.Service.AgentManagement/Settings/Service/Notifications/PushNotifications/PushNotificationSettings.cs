using JetBrains.Annotations;

namespace Lykke.Service.AgentManagement.Settings.Service.Notifications.PushNotifications
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class PushNotificationSettings
    {
        public string AgentApprovedTemplateId { get; set; }

        public string AgentRejectedTemplateId { get; set; }
    }
}
