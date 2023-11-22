using blog_backend.DAO.Model;
using blog_backend.Entity;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.Service.Repository;

public interface IAccountRepository
{
    public User Register(AuthorizationDTO request);

    public void EditUser(EditAccountDTO user);
    
    public void LogoutUser(String token);
    
    
    public User? GetUserById(Guid id);

    public User? GetUserByEmail(string email);
}