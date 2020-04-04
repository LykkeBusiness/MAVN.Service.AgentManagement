namespace MAVN.Service.AgentManagement.Domain.Exceptions
{
    public class SalesforceImageUploadFailException : FailedOperationException
    {
        public SalesforceImageUploadFailException()
            : base("An error occurred while uploading images to the Salesforce when registering agent account.")
        {
        }
    }
}
