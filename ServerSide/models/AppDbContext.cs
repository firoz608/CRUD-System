using Microsoft.EntityFrameworkCore;

namespace CrudApi.models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<crud> crud_data { get; set; }
    }

}
