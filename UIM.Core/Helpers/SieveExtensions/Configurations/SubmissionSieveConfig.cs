namespace UIM.Core.Helpers.SieveExtensions.Configurations;

public class SubmissionSieveConfig : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapp)
    {
        mapp.Property<Submission>(_ => _.Title).CanSort().CanFilter();
        mapp.Property<Submission>(_ => _.Description).CanSort().CanFilter();
        mapp.Property<Submission>(_ => _.CreatedBy).CanSort().CanSort();
        mapp.Property<Submission>(_ => _.ModifiedBy).CanSort().CanSort();
        mapp.Property<Submission>(_ => _.CreatedDate).CanSort().CanSort();
        mapp.Property<Submission>(_ => _.ModifiedDate).CanSort().CanSort();
    }
}
