using blog_backend.DAO.Model;
using blog_backend.DAO.Repository;
using blog_backend.Entity;
using blog_backend.Service.Repository;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.Service;


public class AuthService 
{
    private readonly IAuthRepository _authRepository;
    private readonly GenerateTokenService _tokenService;

    public AuthService(IAuthRepository authRepository, GenerateTokenService tokenService)
    {
        _authRepository = authRepository;
        _tokenService = tokenService;
    }
    
    public string RegisterUser(AuthorizationDTO request)
    {
        if (_authRepository.GetUserByEmail(request.Email) != null)
        {
            throw new ArgumentException("User already exists");
        }

        var user = _authRepository.Register(request);
        var token = _tokenService.GenerateToken(user);
        return  token;
    }

    public string LoginUser(LoginDTO request)
    {
        var user = _authRepository.GetUserByEmail(request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        { 
            throw new ArgumentException("Invalid email or password");
        }

        var token = _tokenService.GenerateToken(user);
        return token;
    }
}