using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.AgentManagement.Settings.Service.Db
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnectionString { get; set; }
        
        public string DataConnectionString { get; set; }
    }
}
