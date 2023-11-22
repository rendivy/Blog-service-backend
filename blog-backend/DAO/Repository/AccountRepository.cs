using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Service;
using blog_backend.Service.Repository;

namespace blog_backend.DAO.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly UserMapper _userMapper = new();
    private readonly BlogDbContext _dbContext;

    public AccountRepository(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public User Register(AuthorizationDTO request)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = _userMapper.MapFromAuthorizationDto(request, passwordHash);
        return user;
    }

    public void EditUser(EditAccountDTO body)
    {
        var user = GetUserByEmail(body.Email);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }
        MapEditAccountDtoToUser(body, user);
        _dbContext.User.Update(user);
        _dbContext.SaveChanges();
    }

    private void MapEditAccountDtoToUser(EditAccountDTO dto, User user)
    {
        user.FullName = dto.FullName;
        user.Gender = dto.Gender;
        user.PhoneNumber = dto.PhoneNumber;
        user.DateOfBirth = dto.BirthDate;
    }

    public User? GetUserById(Guid id)
    {
        return _dbContext.User.FirstOrDefault(u => u.Id == id);
    }

    public User? GetUserByEmail(string email)
    {
        return _dbContext.User.FirstOrDefault(u => u.Email == email);
    }
}