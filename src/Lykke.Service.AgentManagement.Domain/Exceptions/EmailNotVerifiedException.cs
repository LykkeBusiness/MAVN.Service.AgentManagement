namespace Lykke.Service.AgentManagement.Domain.Exceptions
{
    public class EmailNotVerifiedException : FailedOperationException
    {
        public EmailNotVerifiedException()
            : base("Customer email is not verified.")
        {
        }
    }
}
