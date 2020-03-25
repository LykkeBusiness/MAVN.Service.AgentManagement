using JetBrains.Annotations;

namespace Lykke.Service.AgentManagement.Settings.Service.Notifications.EmailNotifications
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class EmailTemplateSettings
    {
        public string SubjectTemplateId { get; set; }

        public string MessageTemplateId { get; set; }
    }
}
