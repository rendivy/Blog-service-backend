using blog_backend.DAO.Model;
using blog_backend.DAO.Repository;
using blog_backend.Entity;
using blog_backend.Service.Mappers;
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

    public async Task EditUser(EditAccountDTO request, string userEmail, string userId)
    {
        var isEmailBusy = _accountRepository.GetUserByEmail(request.Email).Result;
        if (isEmailBusy == null)
        {
            var user = _accountRepository.GetUserById(userId).Result;
            var mappedCurrentUser = EditDtoMapper.Map(request, user);
            await Task.FromResult(_accountRepository.EditUser(mappedCurrentUser, userId));
        }
        else
        {
            throw new ArgumentException("Email is already in use") ;
        }
        
        
    }

    public async Task LogoutUser(string token)
    {
        await Task.Run(() => _accountRepository.LogoutUser(token));
    }


    public async Task<TokenDTO> RegisterUser(AuthorizationDTO request)
    {
        if (_accountRepository.GetUserByEmail(request.Email).Result != null)
        {
            throw new ArgumentException("User already exists");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = _accountRepository.Register(request, passwordHash);
        return await Task.FromResult(user.Result);
    }

    public string LoginUser(LoginDTO request)
    {
        var user = _accountRepository.GetUserByEmail(request.Email).Result;

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            throw new ArgumentException("Invalid email or password");
        }

        var token = _tokenService.GenerateToken(user);
        return token;
    }
}