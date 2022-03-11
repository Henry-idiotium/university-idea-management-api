namespace UIM.Core.Helpers.SieveExtensions.Configurations;

public class DepartmentSieveConfig : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapp)
    {
        mapp.Property<Department>(_ => _.Name).CanFilter().CanSort();
        mapp.Property<Department>(_ => _.CreatedBy).CanFilter().CanSort();
        mapp.Property<Department>(_ => _.CreatedDate).CanFilter().CanSort();
        mapp.Property<Department>(_ => _.ModifiedBy).CanFilter().CanSort();
        mapp.Property<Department>(_ => _.ModifiedDate).CanFilter().CanSort();
    }
}