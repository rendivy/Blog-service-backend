using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Entity.AccountEntities;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.Service.Repository;

public interface IAccountRepository
{
    public Task<TokenDTO> Register(AuthorizationDTO user, string hashPassword);
    
    public Task GetUserName(string userName);
    
    public Task<List<User>> GetAuthors();
    
    public Task EditUser(User user, string userId);

    public Task<User?> GetUserById(string id);
    
    public Task LogoutUser(string token);

    public Task<User?> GetUserByEmail(string userEmail);
}