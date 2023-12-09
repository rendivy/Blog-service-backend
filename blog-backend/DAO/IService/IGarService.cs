using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Service;

namespace blog_backend.DAO.IService;

public interface IGarService
{
    public Task<List<AddressDTO>> GetAllHierarchy(Guid objectGuid);

    public Task<List<AddressDTO>> SearchAddressAsync(int parentObjectId, string query);
}