using Microsoft.EntityFrameworkCore;
using PET1.Domain.Entities;

namespace PET1.API.Data
{
    public class AppDbContext: DbContext
    {
       public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Movies> Movies { get; set; }
    }
}
