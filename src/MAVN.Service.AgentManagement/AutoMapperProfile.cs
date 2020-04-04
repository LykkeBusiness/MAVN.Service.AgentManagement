using AutoMapper;
using MAVN.Service.AgentManagement.Client.Models.Agents;
using MAVN.Service.AgentManagement.Client.Models.Requirements;
using MAVN.Service.AgentManagement.Domain.Entities.Agents;
using MAVN.Service.AgentManagement.Domain.Entities.Registration;
using MAVN.Service.AgentManagement.Domain.Entities.Requirements;

namespace MAVN.Service.AgentManagement
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
