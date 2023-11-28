using blog_backend.DAO.Database;
using blog_backend.Entity;
using blog_backend.Service.Repository;

namespace blog_backend.DAO.Repository;

public class TagsRepository : ITagsRepository
{
    private readonly BlogDbContext _blogDbContext;

    public TagsRepository(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public Task<List<Tag>> GetTags()
    {
        return Task.FromResult(_blogDbContext.Tags.ToList());
    }

    public Task<Tag> CreateTag(Tag tagDto)
    {
        var tag = new Tag { Name = tagDto.Name };
        _blogDbContext.Tags.Add(tag);
        _blogDbContext.SaveChanges();
        return Task.FromResult(tag);
    }
}