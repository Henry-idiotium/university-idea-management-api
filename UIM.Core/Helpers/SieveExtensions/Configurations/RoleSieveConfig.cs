namespace UIM.Core.Helpers.SieveExtensions.Configurations;

public class RoleSieveConfig : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapp)
    {
        mapp.Property<IdentityRole>(_ => _.Name).CanFilter().CanSort();
    }
}
