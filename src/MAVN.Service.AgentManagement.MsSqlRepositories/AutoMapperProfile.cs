using AutoMapper;
using MAVN.Service.AgentManagement.Domain.Entities.Agents;
using MAVN.Service.AgentManagement.MsSqlRepositories.Entities;

namespace MAVN.Service.AgentManagement.MsSqlRepositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Agent, AgentEntity>(MemberList.Source);
            CreateMap<AgentEntity, Agent>(MemberList.Destination);

            CreateMap<EncryptedImage, ImageEntity>(MemberList.Source)
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Hash));
        }
    }
}
