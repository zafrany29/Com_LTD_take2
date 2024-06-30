using Microsoft.EntityFrameworkCore;
using Welp.Models;

namespace Welp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CompanyInfo> CompanyInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // הגדרת עמודת PasswordHistory כ-TEXT
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.PasswordHistory)
                    .HasColumnType("TEXT")
                    .HasDefaultValue("[]");
            });
        }
    }
}