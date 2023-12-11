using AutoMapper;
using blog_backend.DAO.Model;
using blog_backend.Entity.AccountEntities;

namespace blog_backend.ApplicationMapper;

public class AppMappingProfile : Profile
{
    private readonly string _hashPassword;

   
    public AppMappingProfile()
    {
        
        _hashPassword = null;

        
        CreateMap<AuthorizationDTO, User>()
            .ForMember(u => u.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(u => u.Password, opt => opt.MapFrom(_ => _hashPassword));
    }


    public AppMappingProfile(string hashPassword) : this()
    {
        _hashPassword = hashPassword;
    }
}