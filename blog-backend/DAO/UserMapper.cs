using blog_backend.Dao.Repository.Model;
using blog_backend.Entity;


namespace blog_backend.DAO;

public class UserMapper
{
    public User MapFromAuthorizationDto(UserAuthorizationDto authorizationDto)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = authorizationDto.Email,
            Password = authorizationDto.Password,
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
            Password = user.Password,
            Name = user.Name
        };
    }
}


