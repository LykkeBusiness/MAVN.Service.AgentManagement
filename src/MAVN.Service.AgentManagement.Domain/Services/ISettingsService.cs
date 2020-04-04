using Falcon.Numerics;

namespace MAVN.Service.AgentManagement.Domain.Services
{
    public interface ISettingsService
    {
        string GetTokenName();

        bool IsDemoEmail(string email);
    }
}
