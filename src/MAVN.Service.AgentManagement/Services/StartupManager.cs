using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common;
using Lykke.Common.Log;
using Lykke.Sdk;

namespace MAVN.Service.AgentManagement.Services
{
    public class StartupManager : IStartupManager
    {
        private readonly IEnumerable<IStartStop> _startables;
        private readonly ILog _log;

        public StartupManager(
            IEnumerable<IStartStop> startables,
            ILogFactory logFactory)
        {
            _startables = startables;
            _log = logFactory.CreateLog(this);
        }

        public Task StartAsync()
        {
            foreach (var startable in _startables)
            {
                startable.Start();
            }

            return Task.CompletedTask;
        }
    }
}
