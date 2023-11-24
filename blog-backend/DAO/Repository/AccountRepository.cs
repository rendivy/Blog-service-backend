using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using blog_backend.Service.Repository;


namespace blog_backend.DAO.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly UserMapper _userMapper = new();
    private readonly BlogDbContext _dbContext;
    private readonly GenerateTokenService _tokenService;

    public AccountRepository(BlogDbContext dbContext, GenerateTokenService tokenService)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
    }


    public User Register(AuthorizationDTO request)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = _userMapper.MapFromAuthorizationDto(request, passwordHash);
        return user;
    }

   


    public void EditUser(EditAccountDTO body, string userId)
    {
        var user = GetUserById(userId);
        if (user.Result == null)
        {
            throw new ArgumentException("User not found");
        }

        MapEditAccountDtoToUser(body, user.Result);
        _dbContext.User.Update(user.Result);
        _dbContext.SaveChanges();
    }

    public Task<User?> GetUserById(string id)
    {
        return Task.FromResult(_dbContext.User.FirstOrDefault(u => u.Id.ToString() == id));
    }

    public void LogoutUser(string token)
    {
        _tokenService.SaveExpiredToken(token);
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

    public async Task<User?> GetUserByEmail(string userEmail)
    {
        return await Task.FromResult(_dbContext.User.FirstOrDefault(u => u.Email == userEmail));
    }
}