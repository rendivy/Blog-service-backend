using blog_backend.DAO.Model;
using blog_backend.DAO.Utils;
using blog_backend.Entity;

namespace blog_backend.Service.Mappers;

public class CommunityMapper
{
    private static UserAccountDto MapUser(User user)
    {
        return new UserAccountDto(user.Id,
            user.FullName, user.Gender, user.CreateTime,
            user.Email, user.PhoneNumber, user.DateOfBirth);
    }

    public static CommunityListDTO MapToList(Community community, string userId, string role)
    {
        var communityDto = new CommunityListDTO { UserId = new Guid(userId), CommunityId = community.Id, Role = role };
        return communityDto;
    }
    
    
    public static CommunityDTO MapCommunityDto(List<PostDTO> posts, PaginationDTO paginationDTO)
    {
        return new CommunityDTO
        {
            Posts = posts,
            Pagination = paginationDTO
        };
    }


    public static CommunityShortDTO MapToShort(Community community)
    {
        return new CommunityShortDTO(
            community.Id,
            community.CreateTime,
            community.Name,
            community.Description,
            community.IsClosed,
            community.SubscribersCount
        );
    }

    public static CommunityDetailsDTO MapToDetails(Community community)
    {
        return new CommunityDetailsDTO
        {
            Id = community.Id,
            CreateTime = community.CreateTime,
            Name = community.Name,
            Description = community.Description,
            IsClosed = community.IsClosed,
            SubscribersCount = community.SubscribersCount,
            Administrators = community.Memberships
                .Where(membership => membership.RoleEnum == RoleEnum.Administrator)
                .Select(membership => MapUser(membership.User))
                .ToList()
        };
    }

    public static Community Map(CreateCommunityDTO communityDTO, User admin)
    {
        return new Community
        {
            Id = new Guid(),
            Name = communityDTO.Name,
            SubscribersCount = 1,
            Description = communityDTO.Description,
            IsClosed = communityDTO.IsClosed,
            Memberships = new List<CommunityMembership>
            {
                new()
                {
                    UserId = admin.Id,
                    RoleEnum = RoleEnum.Administrator
                }
            },
        };
    }
}