using AutoMapper;
using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.DAO.Repository;
using blog_backend.Entity;
using blog_backend.Entity.AccountEntities;
using blog_backend.Service.Mappers;
using blog_backend.Service.Repository;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.Service;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly GenerateTokenService _tokenService;
    private readonly BlogDbContext _dbContext;
    private readonly IMapper _mapper;


    public AccountService(IAccountRepository accountRepository, GenerateTokenService tokenService,
        BlogDbContext dbContext,  IMapper mapper)
    {
        _accountRepository = accountRepository;
        _tokenService = tokenService;
        _dbContext = dbContext;
        _mapper = mapper;
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
            throw new ArgumentException("Email is already in use");
        }
    }

    public async Task LogoutUser(string token)
    {
        await _accountRepository.LogoutUser(token);
    }


    public async Task<TokenDTO> RegisterUser(AuthorizationDTO request)
    {
        var existingUser = await _accountRepository.GetUserByEmail(request.Email);
        if (existingUser != null)
        {
            throw new ArgumentException("User already exists");
        }

        var hashPassword =  BCrypt.Net.BCrypt.HashPassword(request.Password);
        var token = await _accountRepository.Register(request, hashPassword);
        return  token;
    }

    public async Task<string> LoginUser(LoginDTO request)
    {
        var user = _accountRepository.GetUserByEmail(request.Email).Result;

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            throw new ArgumentException("Invalid email or password");
        }

        var token = await _tokenService.GenerateToken(user);
        return token;
    }
}