using blog_backend.Dao.Repository.Model;
using blog_backend.Entity;


namespace blog_backend.DAO;

public class UserMapper
{
    public User MapFromAuthorizationDto(AuthorizationDTO authorizationDto, string hashPassword)
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

    public AuthorizationDTO MapToAuthorizationDto(User user)
    {
        return new AuthorizationDTO
        {
            Gender = user.Gender,
            Email = user.Email,
            Password = user.Password,
            FullName = user.FullName,
            DateOfBirth = user.DateOfBirth,
            PhoneNumber = user.PhoneNumber
        };
    }
}


