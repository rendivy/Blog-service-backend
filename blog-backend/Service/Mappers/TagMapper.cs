using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Entity.PostEntities;

namespace blog_backend.Service.Mappers;

public class TagMapper
{
    public static TagDTO MapToDTO(Tag tag)
    {
        return new TagDTO
        {
            Id = tag.Id,
            Name = tag.Name,
            CreateTime = tag.CreateTime
        };
    }
}