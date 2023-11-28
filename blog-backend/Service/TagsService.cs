using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Service.Repository;

namespace blog_backend.Service;

public class TagsService
{
    private readonly ITagsRepository _tagsRepository;

    public TagsService(ITagsRepository tagsRepository)
    {
        _tagsRepository = tagsRepository;
    }

    public async Task<Tag> CreateTag(CreateTagDTO tagDto)
    {
        var tag = new Tag { Name = tagDto.Name};
        return await _tagsRepository.CreateTag(tag);
    }

    public async Task<List<Tag>> GetTags()
    {
        return await _tagsRepository.GetTags();
    }
}