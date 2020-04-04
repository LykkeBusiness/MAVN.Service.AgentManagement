namespace MAVN.Service.AgentManagement.Domain.Exceptions
{
    public class AlreadyApprovedException : FailedOperationException
    {
        public AlreadyApprovedException()
            : base("Customer already registered and approved as an agent.")
        {
        }
    }
}
