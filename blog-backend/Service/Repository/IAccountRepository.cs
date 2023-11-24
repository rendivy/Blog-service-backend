using blog_backend.DAO.Model;
using blog_backend.Entity;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.Service.Repository;

public interface IAccountRepository
{
    public User Register(AuthorizationDTO request);
    
    
    public void EditUser(EditAccountDTO user, string userId);

    public Task<User?> GetUserById(string id);
    
    public void LogoutUser(string token);

    public Task<User?> GetUserByEmail(string userEmail);
}