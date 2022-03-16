namespace UIM.Core.Data;

public class UimContext : IdentityDbContext<AppUser>
{
    public UimContext(DbContextOptions<UimContext> options) : base(options) { }

    public DbSet<Idea> Ideas => Set<Idea>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<View> Views => Set<View>();
    public DbSet<Models.Entities.Attachment> Attachments => Set<Models.Entities.Attachment>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

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

            conf.OwnsMany(_ => _.Attachments, b =>
            {
                b.Property(_ => _.Id).HasMaxLength(450);
                b.Property(_ => _.IdeaId).IsRequired();
            });

            conf.OwnsMany(_ => _.Comments, b =>
            {
                b.Property(_ => _.Id).HasMaxLength(450);
                b.Property(_ => _.IdeaId).IsRequired();
                b.Property(_ => _.Content).IsRequired();
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
        {
            conf.Property(_ => _.Id).HasMaxLength(450);
            conf.Property(_ => _.Name).IsRequired();
        });

        builder.Entity<Category>(conf =>
        {
            conf.Property(_ => _.Id).HasMaxLength(450);
            conf.Property(_ => _.Name).IsRequired().HasMaxLength(450);
        });
    }
}