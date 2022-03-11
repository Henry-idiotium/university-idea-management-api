namespace UIM.Core.Helpers.SieveExtensions.Configurations;

public class UserSieveConfig : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapp)
    {
        mapp.Property<AppUser>(_ => _.UserName).CanFilter().CanSort();
        mapp.Property<AppUser>(_ => _.FullName).CanFilter().CanSort();
        mapp.Property<AppUser>(_ => _.Email).CanFilter().CanSort();
        mapp.Property<AppUser>(_ => _.CreatedDate).CanFilter().CanSort();
        mapp.Property<AppUser>(_ => _.DateOfBirth).CanFilter().CanSort()
            .HasName("birth_date");
    }
}