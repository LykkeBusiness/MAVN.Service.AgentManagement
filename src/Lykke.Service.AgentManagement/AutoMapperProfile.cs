using AutoMapper;
using Lykke.Service.AgentManagement.Client.Models.Agents;
using Lykke.Service.AgentManagement.Client.Models.Requirements;
using Lykke.Service.AgentManagement.Domain.Entities.Agents;
using Lykke.Service.AgentManagement.Domain.Entities.Registration;
using Lykke.Service.AgentManagement.Domain.Entities.Requirements;

namespace Lykke.Service.AgentManagement
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Agent, AgentModel>(MemberList.Source);

            CreateMap<ImageModel, Image>(MemberList.Destination);
            CreateMap<RegistrationModel, RegistrationForm>(MemberList.Destination)
                .ForMember(x => x.CountryPhoneCodeId, opt => opt.Ignore())
                .ForMember(x => x.PhoneNumber, opt => opt.Ignore());

            CreateMap<CustomerRequirements, CustomerRequirementsModel>(MemberList.Source);
        }
    }
}
