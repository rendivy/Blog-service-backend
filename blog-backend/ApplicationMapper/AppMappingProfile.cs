using AutoMapper;
using blog_backend.DAO.Model;
using blog_backend.DAO.Model.Account;
using blog_backend.Entity.AccountEntities;
using blog_backend.Entity.PostEntities;

namespace blog_backend.ApplicationMapper;

public class AppMappingProfile : Profile
{
    private readonly string _hashPassword;


    public AppMappingProfile()
    {
        _hashPassword = null;
        CreateMap<EditAccountDTO, User>();
        CreateMap<Tag, TagDTO>();
        CreateMap<TagDTO, Tag>();
        CreateMap<CreateTagDTO, Tag>().ForMember(u => u.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));
        CreateMap<User, UserAccountDto>();
        CreateMap<RegistrationDTO, User>()
            .ForMember(u => u.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(u => u.Password, opt => opt.MapFrom(_ => _hashPassword));
    }


    public AppMappingProfile(string hashPassword) : this()
    {
        _hashPassword = hashPassword;
    }
}