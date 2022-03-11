namespace UIM.Core.Helpers.SieveExtensions.Configurations;

public class CategorySieveConfig : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapp)
    {
        mapp.Property<Category>(_ => _.Name).CanFilter().CanSort();
        mapp.Property<Category>(_ => _.CreatedBy).CanFilter().CanSort();
        mapp.Property<Category>(_ => _.CreatedDate).CanFilter().CanSort();
        mapp.Property<Category>(_ => _.ModifiedBy).CanFilter().CanSort();
        mapp.Property<Category>(_ => _.ModifiedDate).CanFilter().CanSort();
    }
}