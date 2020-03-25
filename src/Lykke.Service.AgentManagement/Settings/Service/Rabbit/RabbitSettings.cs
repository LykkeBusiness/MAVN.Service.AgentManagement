using JetBrains.Annotations;
using Lykke.Service.AgentManagement.Settings.Service.Rabbit.Publishers;

namespace Lykke.Service.AgentManagement.Settings.Service.Rabbit
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RabbitSettings
    {
        public RabbitPublishers Publishers { get; set; }
    }
}
