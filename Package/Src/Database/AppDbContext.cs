using Microsoft.EntityFrameworkCore;

namespace ViteReact.Database
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        { }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        { }
    }
}
