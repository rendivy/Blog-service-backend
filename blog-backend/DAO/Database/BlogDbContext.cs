using blog_backend.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace blog_backend.DAO.Database;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<Community> Communities { get; set; } = null!;
    public DbSet<CommunityMembership> CommunityMemberships { get; set; } = null!;
    public DbSet<ExpiredToken> ExpiredTokens { get; set; } = null!;
    public DbSet<Post?> Posts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommunityMembership>()
            .HasKey(cm => new { cm.UserId, cm.CommunityId });
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        var connectionString = configuration.GetConnectionString("WebApiDatabase");
        optionsBuilder.UseNpgsql(connectionString);
    }
}