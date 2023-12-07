using AutoMapper;
using Solvintech.Application.DTO.User;
using Solvintech.Infrastructure.Data.Entities;

namespace Solvintech.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, UserDto>(MemberList.Destination);
        CreateMap<UserDto, ApplicationUser>(MemberList.Source);
        CreateMap<LoginUserDto, ApplicationUser>(MemberList.Source)
            .ForMember(x => x.UserName, x => x.MapFrom(s => s.Email));
        CreateMap<RegisterUserDto, ApplicationUser>(MemberList.Source)
            .ForMember(x => x.UserName, x => x.MapFrom(s => s.Email));
    }
}