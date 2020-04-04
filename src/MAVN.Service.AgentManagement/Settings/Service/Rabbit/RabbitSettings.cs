using JetBrains.Annotations;
using MAVN.Service.AgentManagement.Settings.Service.Rabbit.Publishers;

namespace MAVN.Service.AgentManagement.Settings.Service.Rabbit
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RabbitSettings
    {
        public RabbitPublishers Publishers { get; set; }
    }
}
