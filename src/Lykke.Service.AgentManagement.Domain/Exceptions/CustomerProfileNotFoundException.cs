namespace Lykke.Service.AgentManagement.Domain.Exceptions
{
    public class CustomerProfileNotFoundException : FailedOperationException
    {
        public CustomerProfileNotFoundException()
            : base("Customer profile does not exist.")
        {
        }
    }
}
