using blog_backend.DAO.Model;
using blog_backend.DAO.Model.Account;
using blog_backend.Entity;
using blog_backend.Entity.AccountEntities;

namespace blog_backend.Service.Mappers;

public class AuthDtoMapper
{
    public static User Map(RegistrationDTO registrationDto, string hashPassword)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = registrationDto.Email,
            Password = hashPassword,
            Gender = registrationDto.Gender,
            FullName = registrationDto.FullName,
            DateOfBirth = registrationDto.DateOfBirth,
            PhoneNumber = registrationDto.PhoneNumber
        };
    }
}