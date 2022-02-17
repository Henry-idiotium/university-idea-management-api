using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UIM.Model.Entities;

namespace UIM.DAL.Data
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Ignore<IdentityUserLogin<string>>();
            builder.Ignore<IdentityUserClaim<string>>();
            builder.Ignore<IdentityUserToken<string>>();
            builder.Ignore<IdentityRoleClaim<string>>();

            builder.Entity<AppUser>().Property(_ => _.DateOfBirth).HasColumnType("date");

            builder.Entity<Like>().HasKey(_ => new { _.UserId, _.IdeaId });
            builder.Entity<Like>().Property(_ => _.IsLike).IsRequired();

            builder.Entity<Comment>().Property(_ => _.IdeaId).IsRequired();
            builder.Entity<Comment>().Property(_ => _.Content).IsRequired();

            builder.Entity<Submission>().Property(_ => _.InitialDate).IsRequired();
            builder.Entity<Submission>().Property(_ => _.FinalDate).IsRequired();
            builder.Entity<Submission>().Property(_ => _.Title).IsRequired();

            builder.Entity<Department>().Property(_ => _.Name).IsRequired();

            builder.Entity<Category>().Property(_ => _.Name).IsRequired();

            // Relationships
            // Likes - User
            builder.Entity<Like>()
                .HasOne(_ => _.User)
                .WithMany(_ => _.Likes)
                .HasForeignKey(_ => _.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Likes - Idea
            builder.Entity<Like>()
                .HasOne(_ => _.Idea)
                .WithMany(_ => _.Likes)
                .HasForeignKey(_ => _.IdeaId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // View
            builder.Entity<View>()
                .HasKey(_ => new { _.UserId, _.IdeaId });

            // Views - User
            builder.Entity<View>()
                .HasOne(_ => _.User)
                .WithMany(_ => _.Views)
                .HasForeignKey(_ => _.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Views - Idea
            builder.Entity<View>()
                .HasOne(_ => _.Idea)
                .WithMany(_ => _.Views)
                .HasForeignKey(_ => _.IdeaId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<RefreshToken>(conf =>
            {
                conf.HasOne(_ => _.User).WithMany(_ => _.RefreshTokens);
                conf.Property(_ => _.Token).HasMaxLength(100);
                conf.Property(_ => _.UserId).IsRequired();
                conf.Property(_ => _.ReplacedByToken).HasMaxLength(100);
                conf.Property(_ => _.ReasonRevoked).HasMaxLength(500);
            });
        }
    }
}