using blog_backend.DAO;
using blog_backend.DAO.Repository;
using blog_backend.Dao.Repository.Model;
using blog_backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace blog_backend.Service;

public class UserRepository : IUserRepository
{

    private readonly UserMapper _userMapper = new();
    
    
    public List<User> GetAllUsers()
    {
        using var context = new BlogDbContext(new DbContextOptions<BlogDbContext>());
        return context.User.ToList();
    }

    public void AddUser(UserAuthorizationDTO user)
    {
        using var context = new BlogDbContext(new DbContextOptions<BlogDbContext>());
        context.User.Add(_userMapper.MapFromAuthorizationDto(user));
        context.SaveChanges();
    }
    
}