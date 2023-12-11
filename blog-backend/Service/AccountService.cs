using AutoMapper;
using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.DAO.Repository;
using blog_backend.Entity;
using blog_backend.Entity.AccountEntities;
using blog_backend.Service.Extensions;
using blog_backend.Service.Mappers;
using blog_backend.Service.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog_backend.Service;

public class AccountService
{
    private readonly GenerateTokenService _tokenService;
    private readonly BlogDbContext _dbContext;
    private readonly IMapper _mapper;


    public AccountService(GenerateTokenService tokenService, BlogDbContext dbContext, IMapper mapper)
    {
        _tokenService = tokenService;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<UserAccountDto> GetUserInfo(string userId)
    {
        var user = await userId.GetUserById(_dbContext);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        var userInfoModel = _mapper.Map<UserAccountDto>(user);
        return userInfoModel;
    }

    public async Task EditUser(EditAccountDTO request, string userEmail, string userId)
    {
        var isEmailBusy = await userEmail.GetUserByEmail(_dbContext);
        if (isEmailBusy == null || isEmailBusy.Id == new Guid(userId))
        {
            var user = await userId.GetUserById(_dbContext);
            var mappedCurrentUser = _mapper.Map(request, user);
            _dbContext.User.Update(mappedCurrentUser);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentException("Email is already in use");
        }
    }

    public async Task LogoutUser(string token)
    {
        await _tokenService.SaveExpiredToken(token);
    }


    public async Task<TokenDTO> RegisterUser(AuthorizationDTO request)
    {
        if (await request.Email.GetUserByEmail(_dbContext) != null)
        {
            throw new ArgumentException("User already exists");
        }

        try
        {
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = _mapper.Map<User>(request, opt =>
                opt.AfterMap((src, dest) => dest.Password = hashPassword));
            await _dbContext.User.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return new TokenDTO { Token = await _tokenService.GenerateToken(user) };
        }
        catch (Exception e)
        {
            throw new ArgumentException($"{e.Message}");
        }
        
    }

    public async Task<string> LoginUser(LoginDTO request)
    {
        var user = await request.Email.GetUserByEmail(_dbContext);

        if (user == null)
        {
            throw new ArgumentException("Invalid email or password");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            throw new ArgumentException("Invalid email or password");
        }

        var token = await _tokenService.GenerateToken(user);
        return token;
    }
}