namespace Lykke.Service.AgentManagement.Domain.Exceptions
{
    public class SalesforceAccountAlreadyExistsException : FailedOperationException
    {
        public SalesforceAccountAlreadyExistsException()
            : base("Customer already registered in Salesforce.")
        {
        }
    }
}
