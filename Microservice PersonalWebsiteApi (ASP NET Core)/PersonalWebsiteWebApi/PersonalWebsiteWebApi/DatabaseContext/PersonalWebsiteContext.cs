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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
