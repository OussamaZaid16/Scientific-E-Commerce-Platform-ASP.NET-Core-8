using GererUser.Models;
using GestionUsersMVC.Models;
using Microsoft.EntityFrameworkCore;
using ScienceStore.Models;

namespace GestionUsersMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        // Dans votre fichier Data/AppDbContext.cs
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure la relation Many-to-Many
            modelBuilder.Entity<Commande>()
                .HasMany(c => c.Articles)
                .WithMany();
        }
    }
}
