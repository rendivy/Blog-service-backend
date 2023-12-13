using AutoMapper;
using blog_backend.DAO.Database;
using blog_backend.DAO.Model;
using blog_backend.Entity.PostEntities;
using blog_backend.Service.Mappers;
using blog_backend.Service.Repository;
using Microsoft.EntityFrameworkCore;

namespace blog_backend.Service;

public class TagsService
{
    
    private readonly BlogDbContext _blogDbContext;
    private readonly IMapper _mapper;


    public TagsService(BlogDbContext blogDbContext, IMapper mapper)
    {
        _blogDbContext = blogDbContext;
        _mapper = mapper;
    }

    public async Task CreateTag(CreateTagDTO tagDto)
    {
        var tagEntity = _mapper.Map<Tag>(tagDto);
        await _blogDbContext.Tags.AddAsync(tagEntity);
        await _blogDbContext.SaveChangesAsync();
    }

    public async Task<List<TagDTO>> GetTags()
    {
        var tags = await _blogDbContext.Tags.ToListAsync();
        return tags.Select(tag => _mapper.Map<TagDTO>(tag)).ToList();
    }
}