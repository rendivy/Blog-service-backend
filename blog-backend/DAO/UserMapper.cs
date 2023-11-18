using blog_backend.Dao.Repository.Model;
using blog_backend.Entity;


namespace blog_backend.DAO;

public class UserMapper
{
    public User MapFromAuthorizationDto(UserAuthorizationDto authorizationDto, string hashPassword)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = authorizationDto.Email,
            PasswordHash = hashPassword,
            Gender = authorizationDto.Gender,
            Name = authorizationDto.Name,
        };
    }

    public UserAuthorizationDto MapToAuthorizationDto(User user)
    {
        return new UserAuthorizationDto
        {
            Gender = user.Gender,
            Email = user.Email,
            Password = user.PasswordHash,
            Name = user.Name
        };
    }
}


