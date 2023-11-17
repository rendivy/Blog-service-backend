using blog_backend.Dao.Repository.Model;
using blog_backend.Entity;


namespace blog_backend.DAO;

public class UserMapper
{
    public User MapFromAuthorizationDto(UserAuthorizationDTO authorizationDto)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = authorizationDto.Email,
            Password = authorizationDto.Password,
            Gender = authorizationDto.Gender,
            Name = null, 
            Surname = null
        };
    }

    public UserAuthorizationDTO MapToAuthorizationDto(User user)
    {
        return new UserAuthorizationDTO
        {
            Gender = user.Gender,
            Email = user.Email,
            Password = user.Password
        };
    }
}


