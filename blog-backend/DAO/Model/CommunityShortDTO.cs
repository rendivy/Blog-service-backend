namespace blog_backend.DAO.Model;

public class CommunityShortDTO
{
    public CommunityShortDTO(Guid id, DateTime createTime, string name, string description, bool isClosed, int subscribersCount)
    {
        Id = id;
        CreateTime = createTime;
        Name = name;
        Description = description;
        IsClosed = isClosed;
        SubscribersCount = subscribersCount;
    }
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsClosed { get; set; }
    public int SubscribersCount { get; set; }
}
