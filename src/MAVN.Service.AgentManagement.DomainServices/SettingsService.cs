using System.Text.RegularExpressions;
using MAVN.Service.AgentManagement.Domain.Services;

namespace MAVN.Service.AgentManagement.DomainServices
{
    public class SettingsService : ISettingsService
    {
        private readonly string _tokenSymbol;
        private readonly Regex _demoEmailRegex;

        public SettingsService(string tokenSymbol, string demoEmailSuffix)
        {
            _tokenSymbol = tokenSymbol;
            
            _demoEmailRegex = 
                new Regex($"^[a-zA-Z0-9_.+-]+@(?:(?:[a-zA-Z0-9-]+\\.)?[a-zA-Z]+\\.)?({demoEmailSuffix})$");
        }

        public string GetTokenName()
            => _tokenSymbol;

        public bool IsDemoEmail(string email)
            => !string.IsNullOrEmpty(email) && _demoEmailRegex.IsMatch(email);
    }
}
