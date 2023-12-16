using blog_backend.DAO.Model;
using blog_backend.DAO.Model.Account;

namespace blog_backend.DAO.IService;

public interface IAccountService
{
    public Task<UserAccountDto> GetUserInfo(string userId);

    public Task<TokenDTO> LoginUser(LoginDTO request);
    
    public Task<TokenDTO> RegisterUser(RegistrationDTO request);

    public Task EditUser(EditAccountDTO request, string userEmail, string userId);

    public Task LogoutUser(string token);
}