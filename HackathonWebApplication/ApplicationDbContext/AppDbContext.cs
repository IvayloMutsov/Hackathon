using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Proffessors> Proffessors { get; set; }

        public DbSet<Procedures> Procedures { get; set; }
    }
}
