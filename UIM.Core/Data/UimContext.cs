using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UIM.Core.Models.Entities;

namespace UIM.Core.Data
{
    public class UimContext : IdentityDbContext<AppUser>
    {
        public UimContext(DbContextOptions<UimContext> options) : base(options) { }

        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<View> Views { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(conf =>
            {
                conf.Property(_ => _.DateOfBirth).HasColumnType("date");
                conf.Property(_ => _.FullName).HasMaxLength(450);
                conf.HasOne(_ => _.Department).WithMany(_ => _.Users)
                    .OnDelete(DeleteBehavior.SetNull);

                conf.OwnsMany(_ => _.RefreshTokens, b =>
                {
                    b.Property(_ => _.Id).HasMaxLength(450);
                    b.Property(_ => _.Token).HasMaxLength(100);
                    b.Property(_ => _.UserId).IsRequired();
                    b.Property(_ => _.ReplacedByToken).HasMaxLength(100);
                    b.Property(_ => _.ReasonRevoked).HasMaxLength(500);
                });
            });

            builder.Entity<Like>(conf =>
            {
                conf.HasKey(_ => new { _.UserId, _.IdeaId });
                conf.Property(_ => _.IsLike).IsRequired();
                conf.HasOne(_ => _.User).WithMany(_ => _.Likes)
                    .OnDelete(DeleteBehavior.Cascade);
                conf.HasOne(_ => _.Idea).WithMany(_ => _.Likes)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<View>(conf =>
            {
                conf.HasKey(_ => new { _.UserId, _.IdeaId });
                conf.HasOne(_ => _.User).WithMany(_ => _.Views)
                    .OnDelete(DeleteBehavior.Cascade);
                conf.HasOne(_ => _.Idea).WithMany(_ => _.Views)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Idea>(conf =>
            {
                conf.Property(_ => _.Id).HasMaxLength(450);
                conf.HasOne(_ => _.User).WithMany(_ => _.Ideas)
                    .OnDelete(DeleteBehavior.SetNull);
                conf.HasOne(_ => _.Category).WithMany(_ => _.Ideas)
                    .OnDelete(DeleteBehavior.SetNull);
                conf.HasOne(_ => _.Submission).WithMany(_ => _.Ideas)
                    .OnDelete(DeleteBehavior.Cascade);

                conf.OwnsMany(_ => _.Comments, c =>
                {
                    c.Property(_ => _.Id).HasMaxLength(450);
                    c.Property(_ => _.IdeaId).IsRequired();
                    c.Property(_ => _.Content).IsRequired();
                });
            });

            builder.Entity<Submission>(conf =>
            {
                conf.Property(_ => _.Id).HasMaxLength(450);
                conf.Property(_ => _.Title).IsRequired();
                conf.Property(_ => _.InitialDate).IsRequired();
                conf.Property(_ => _.FinalDate).IsRequired();
            });

            builder.Entity<Department>(conf =>
                conf.Property(_ => _.Name).IsRequired());

            builder.Entity<Category>(conf =>
                conf.Property(_ => _.Name).IsRequired());

            builder.Entity<Attachment>(conf =>
                conf.HasOne(_ => _.Idea)
                    .WithMany(_ => _.Attachments)
                    .OnDelete(DeleteBehavior.Cascade));
        }
    }
}