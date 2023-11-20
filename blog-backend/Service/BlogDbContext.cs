using blog_backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace blog_backend.DAO.Database;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }
    
    public DbSet<User> User { get; set; } = null!;
    
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