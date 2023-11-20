using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Service;
using blog_backend.Service.Repository;

namespace blog_backend.DAO.Repository;

public class AuthRepository : IAuthRepository
{
    private readonly UserMapper _userMapper = new();
    private readonly BlogDbContext _dbContext;

    public AuthRepository(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public User Register(AuthorizationDTO request)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        User user = _userMapper.MapFromAuthorizationDto(request, passwordHash);
        return user;
    }

    public User? GetUserByEmail(string email)
    {
        return _dbContext.User.FirstOrDefault(u => u.Email == email);
    }
}