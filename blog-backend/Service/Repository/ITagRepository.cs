using blog_backend.Entity;

namespace blog_backend.Service.Repository;

public interface ITagsRepository
{ 
    public Task<List<Tag>> GetTags();
    
    public Task<Tag> CreateTag(Tag tagDto);
}