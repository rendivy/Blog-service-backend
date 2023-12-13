using blog_backend.Entity;
using blog_backend.Entity.PostEntities;

namespace blog_backend.Service.Repository;

public interface ITagsRepository
{ 
    public Task<List<Tag>> GetTags();
    
    public Task CreateTag(Tag tagDto);
    
    public Task SaveChanges();
}