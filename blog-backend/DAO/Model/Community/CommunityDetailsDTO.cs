namespace blog_backend.DAO.Model;

public class CommunityDetailsDTO
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsClosed { get; set; }
    public int SubscribersCount { get; set; }
    public List<UserAccountDto> Administrators { get; set; } = null!;
}