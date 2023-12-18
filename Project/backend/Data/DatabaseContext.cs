using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.ComponentModel.DataAnnotations.Schema;


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
             modelBuilder.Entity<Database>()
            .Ignore(d => d.DomainEnvironmentBpdatabases);

             modelBuilder.Entity<Database>()
            .Ignore(d => d.DomainEnvironmentEaidatabases);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Database>(entity =>
            {
                entity.HasKey(e => e.DatabaseId); 
            });

        }
    }
}
