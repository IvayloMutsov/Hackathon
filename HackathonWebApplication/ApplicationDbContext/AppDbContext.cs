using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Professors> Professors { get; set; }

        public DbSet<Procedures> Procedures { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=IVAYLO\\SQLEXPRESS;Database=Hakaton;Trusted_Connection=True;TrustServerCertificate = True;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
