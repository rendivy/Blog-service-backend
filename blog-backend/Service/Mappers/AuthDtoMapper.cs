using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Entity.AccountEntities;

namespace blog_backend.Service.Mappers;

public class AuthDtoMapper
{
    public static User Map(AuthorizationDTO authorizationDto, string hashPassword)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = authorizationDto.Email,
            Password = hashPassword,
            Gender = authorizationDto.Gender,
            FullName = authorizationDto.FullName,
            DateOfBirth = authorizationDto.DateOfBirth,
            PhoneNumber = authorizationDto.PhoneNumber
        };
    }
}