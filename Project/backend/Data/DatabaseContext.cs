using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Database> Databases { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Database>(entity =>
            {
                entity.HasKey(e => e.DatabaseId); 
            });

        }
    }
}
