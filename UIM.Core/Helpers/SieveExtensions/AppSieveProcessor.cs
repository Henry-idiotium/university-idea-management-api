namespace UIM.Core.Helpers.SieveExtensions;

public class AppSieveProcessor : SieveProcessor
{
    public AppSieveProcessor(IOptions<SieveOptions> options) : base(options) { }

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        return mapper
            .ApplyConfiguration<UserSieveConfig>()
            .ApplyConfiguration<IdeaSieveConfig>()
            .ApplyConfiguration<TagSieveConfig>()
            .ApplyConfiguration<DepartmentSieveConfig>()
            .ApplyConfiguration<RoleSieveConfig>()
            .ApplyConfiguration<SubmissionSieveConfig>();
    }
}
