namespace MAVN.Service.AgentManagement.Domain.Exceptions
{
    public class SalesforceAccountRegistrationFailException : FailedOperationException
    {
        public SalesforceAccountRegistrationFailException()
            : base("An error occurred while registering agent account in Salesforce.")
        {
        }
    }
}
