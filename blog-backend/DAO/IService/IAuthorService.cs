using blog_backend.DAO.Model;

namespace blog_backend.DAO.IService;

public interface IAuthorService
{
    public Task<List<AuthorDTO>> GetAuthorList();
}