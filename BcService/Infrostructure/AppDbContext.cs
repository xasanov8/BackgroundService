using BcService.Models;
using Microsoft.EntityFrameworkCore;

namespace BcService.Infrostructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<UserModel> Users { get; set; }
    }
}
