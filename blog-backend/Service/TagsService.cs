using blog_backend.DAO.Model;
using blog_backend.Entity.PostEntities;
using blog_backend.Service.Mappers;
using blog_backend.Service.Repository;

namespace blog_backend.Service;

public class TagsService
{
    private readonly ITagsRepository _tagsRepository;

    public TagsService(ITagsRepository tagsRepository)
    {
        _tagsRepository = tagsRepository;
    }

    public Task CreateTag(CreateTagDTO tagDto)
    {
        var tag = new Tag { Name = tagDto.Name };
        var operationStatus = _tagsRepository.CreateTag(tag);
        if (!operationStatus.IsCompletedSuccessfully)
        {
            throw new Exception("Failed to create tag");
        }
        _tagsRepository.SaveChanges();
        return Task.CompletedTask;
    }

    public async Task<List<TagDTO>> GetTags()
    {
        var tags = await _tagsRepository.GetTags();
        return tags.Select(TagMapper.MapToDTO).ToList();
    }
}