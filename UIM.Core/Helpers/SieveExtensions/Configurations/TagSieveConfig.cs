namespace UIM.Core.Helpers.SieveExtensions.Configurations;

public class TagSieveConfig : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapp)
    {
        mapp.Property<Tag>(_ => _.Name).CanFilter().CanSort();
        mapp.Property<Tag>(_ => _.CreatedBy).CanFilter().CanSort();
        mapp.Property<Tag>(_ => _.CreatedDate).CanFilter().CanSort();
        mapp.Property<Tag>(_ => _.ModifiedBy).CanFilter().CanSort();
        mapp.Property<Tag>(_ => _.ModifiedDate).CanFilter().CanSort();
    }
}