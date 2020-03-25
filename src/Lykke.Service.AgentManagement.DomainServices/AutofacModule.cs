using Autofac;
using Lykke.Service.AgentManagement.Domain.Services;

namespace Lykke.Service.AgentManagement.DomainServices
{
    public class AutofacModule : Module
    {
        private readonly string _tokenSymbol;
        private readonly string _demoEmailSuffix;
        private readonly string _agentApprovedTemplateId;
        private readonly string _agentRejectedTemplateId;
        private readonly string _agentRejectedEmailSubjectTemplateId;
        private readonly string _agentRejectedEmailTemplateId;

        public AutofacModule(
            string tokenSymbol,
            string demoEmailSuffix,
            string agentApprovedTemplateId,
            string agentRejectedTemplateId,
            string agentRejectedEmailSubjectTemplateId,
            string agentRejectedEmailTemplateId)
        {
            _tokenSymbol = tokenSymbol;
            _demoEmailSuffix = demoEmailSuffix;
            _agentApprovedTemplateId = agentApprovedTemplateId;
            _agentRejectedTemplateId = agentRejectedTemplateId;
            _agentRejectedEmailSubjectTemplateId = agentRejectedEmailSubjectTemplateId;
            _agentRejectedEmailTemplateId = agentRejectedEmailTemplateId;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AgentService>()
                .As<IAgentService>()
                .SingleInstance()
                .WithParameter("agentApprovedPushTemplateId", _agentApprovedTemplateId)
                .WithParameter("agentRejectedPushTemplateId", _agentRejectedTemplateId)
                .WithParameter("agentRejectedEmailSubjectTemplateId", _agentRejectedEmailSubjectTemplateId)
                .WithParameter("agentRejectedEmailTemplateId", _agentRejectedEmailTemplateId);

            builder.RegisterType<RequirementsService>()
                .As<IRequirementsService>()
                .SingleInstance();

            builder.RegisterType<SettingsService>()
                .As<ISettingsService>()
                .WithParameter("tokenSymbol", _tokenSymbol)
                .WithParameter("demoEmailSuffix", _demoEmailSuffix)
                .SingleInstance();
        }
    }
}
