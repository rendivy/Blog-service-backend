using blog_backend.DAO.Model;
using blog_backend.DAO.Repository;
using blog_backend.Entity;
using blog_backend.Service.Repository;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.Service;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly GenerateTokenService _tokenService;


    public AccountService(IAccountRepository accountRepository, GenerateTokenService tokenService)
    {
        _accountRepository = accountRepository;
        _tokenService = tokenService;
    }
    
    public Task<UserAccountDto> GetUserInfo(string userId)
    {
        var user = _accountRepository.GetUserById(userId).Result;
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }
        var dto = new UserAccountDto(
            user.Id,
            user.FullName,
            user.Gender,
            user.CreateTime,
            user.Email,
            user.PhoneNumber,
            user.DateOfBirth
        );
        return Task.FromResult(dto);
    }

    public async Task EditUser(EditAccountDTO user, string userId)
    {
        await Task.Run(() => _accountRepository.EditUser(user, userId));
    }
    
    public async Task LogoutUser(string token)
    {
        await Task.Run(() => _accountRepository.LogoutUser(token));
    }


    public string RegisterUser(AuthorizationDTO request)
    {
        if (_accountRepository.GetUserByEmail(request.Email) != null)
        {
            throw new ArgumentException("User already exists");
        }

        var user = _accountRepository.Register(request);
        var token = _tokenService.GenerateToken(user);
        return token;
    }

    public string LoginUser(LoginDTO request)
    {
        var user = _accountRepository.GetUserByEmail(request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Result.Password))
        {
            throw new ArgumentException("Invalid email or password");
        }

        var token = _tokenService.GenerateToken(user.Result);
        return token;
    }
}