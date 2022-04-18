namespace UIM.Core.Data;

public class UimContext : IdentityDbContext<AppUser>
{
    public UimContext(DbContextOptions<UimContext> options) : base(options) { }

    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Idea> Ideas => Set<Idea>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<View> Views => Set<View>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<IdeaTag> IdeaTags => Set<IdeaTag>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<Attachment> Attachments => Set<Attachment>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>(
            conf =>
            {
                conf.Property(_ => _.DateOfBirth).HasColumnType("date");
                conf.Property(_ => _.FullName).HasMaxLength(450);
                conf.HasOne(_ => _.Department)
                    .WithMany(_ => _.Users)
                    .OnDelete(DeleteBehavior.SetNull);

#nullable disable
                conf.Property(_ => _.Gender)
                    .HasMaxLength(10)
                    .HasConversion(v => v.ToString(), v => (Gender)Enum.Parse(typeof(Gender), v));
#nullable enable

                conf.OwnsMany(
                    _ => _.RefreshTokens,
                    b =>
                    {
                        b.Property(_ => _.Id).HasMaxLength(450);
                        b.Property(_ => _.Token).HasMaxLength(100);
                        b.Property(_ => _.UserId).IsRequired();
                        b.Property(_ => _.ReplacedByToken).HasMaxLength(100);
                        b.Property(_ => _.ReasonRevoked).HasMaxLength(500);
                    }
                );
            }
        );

        builder.Entity<Like>(
            conf =>
            {
                conf.HasKey(_ => new { _.UserId, _.IdeaId });
                conf.HasOne(_ => _.User).WithMany(_ => _.Likes).OnDelete(DeleteBehavior.Cascade);
                conf.HasOne(_ => _.Idea).WithMany(_ => _.Likes).OnDelete(DeleteBehavior.Cascade);
            }
        );

        builder.Entity<View>(
            conf =>
            {
                conf.HasKey(_ => new { _.UserId, _.IdeaId });
                conf.HasOne(_ => _.User).WithMany(_ => _.Views).OnDelete(DeleteBehavior.Cascade);
                conf.HasOne(_ => _.Idea).WithMany(_ => _.Views).OnDelete(DeleteBehavior.Cascade);
            }
        );

        builder.Entity<Idea>(
            conf =>
            {
                conf.Property(_ => _.Id).HasMaxLength(450);
                conf.HasOne(_ => _.User).WithMany(_ => _.Ideas).OnDelete(DeleteBehavior.SetNull);
                conf.HasOne(_ => _.Submission)
                    .WithMany(_ => _.Ideas)
                    .OnDelete(DeleteBehavior.Cascade);

                conf.OwnsMany(
                    _ => _.Attachments,
                    b =>
                    {
                        b.Property(_ => _.Id).HasMaxLength(450);
                        b.Property(_ => _.IdeaId).IsRequired();
                    }
                );
            }
        );

        builder.Entity<Comment>(
            conf =>
            {
                conf.Property(_ => _.Id).HasMaxLength(450);
                conf.Property(_ => _.Content).IsRequired();
                conf.Property(_ => _.IdeaId).IsRequired();

                conf.HasOne(_ => _.Idea).WithMany(_ => _.Comments).OnDelete(DeleteBehavior.Cascade);

                conf.HasOne(_ => _.User).WithMany(_ => _.Comments).OnDelete(DeleteBehavior.SetNull);
            }
        );

        builder.Entity<Tag>(
            conf =>
            {
                conf.Property(_ => _.Id).HasMaxLength(450);
                conf.Property(_ => _.Name).IsRequired().HasMaxLength(450);
                conf.HasIndex(_ => _.Name).IsUnique();
            }
        );

        builder.Entity<IdeaTag>(
            conf =>
            {
                conf.HasKey(_ => new { _.TagId, _.IdeaId });
                conf.HasOne(_ => _.Tag).WithMany(_ => _.IdeaTags).OnDelete(DeleteBehavior.Cascade);
                conf.HasOne(_ => _.Idea).WithMany(_ => _.IdeaTags).OnDelete(DeleteBehavior.Cascade);
            }
        );

        builder.Entity<Submission>(
            conf =>
            {
                conf.Property(_ => _.Id).HasMaxLength(450);
                conf.Property(_ => _.Title).IsRequired();
                conf.Property(_ => _.InitialDate).IsRequired();
                conf.Property(_ => _.FinalDate).IsRequired();
            }
        );

        builder.Entity<Department>(
            conf =>
            {
                conf.Property(_ => _.Id).HasMaxLength(450);
                conf.Property(_ => _.Name).IsRequired();
                conf.HasIndex(_ => _.Name).IsUnique();
            }
        );
    }
}
