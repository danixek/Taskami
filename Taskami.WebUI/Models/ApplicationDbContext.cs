using Microsoft.EntityFrameworkCore;

namespace Taskami.WebUI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApiKeyModel> ApiKey { get; set; } = null!;
    }
}
