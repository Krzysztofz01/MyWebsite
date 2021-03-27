using Microsoft.EntityFrameworkCore;
using PersonalWebsiteWebApi.Models;

#nullable disable

namespace PersonalWebsiteWebApi.DatabaseContext
{
    public partial class PersonalWebsiteContext : DbContext
    {
        public PersonalWebsiteContext()
        {
        }

        public PersonalWebsiteContext(DbContextOptions<PersonalWebsiteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<GalleryImage> GalleryImages { get; set; }
        public virtual DbSet<GithubProject> GithubProjects { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GalleryImage>(entity =>
            {
                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasPrecision(6)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Display).HasDefaultValueSql("((1))");

                entity.Property(e => e.Filename)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GithubProject>(entity =>
            {
                entity.Property(e => e.CreateDate)
                    .HasPrecision(6)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Display).HasDefaultValueSql("((1))");

                entity.Property(e => e.HtmlUrl)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectCreated).HasPrecision(6);

                entity.Property(e => e.ProjectUpdated).HasPrecision(6);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((0))");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.LastLoginDate)
                    .HasPrecision(6)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastLoginIp)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
