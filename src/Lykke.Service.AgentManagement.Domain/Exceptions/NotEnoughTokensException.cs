namespace Lykke.Service.AgentManagement.Domain.Exceptions
{
    public class NotEnoughTokensException : FailedOperationException
    {
        public NotEnoughTokensException()
            : base("Customer has no enough tokens.")
        {
        }
    }
}
