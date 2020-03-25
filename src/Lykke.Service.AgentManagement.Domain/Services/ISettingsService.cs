using Falcon.Numerics;

namespace Lykke.Service.AgentManagement.Domain.Services
{
    public interface ISettingsService
    {
        string GetTokenName();

        bool IsDemoEmail(string email);
    }
}
