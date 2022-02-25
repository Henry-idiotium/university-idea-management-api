using Sieve.Services;
using UIM.Model.Dtos.User;

namespace UIM.BAL.Helpers.SieveExtensions.Configurations
{
    public class SieveConfigurationForUser : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<UserDetailsResponse>(p => p.UserName)
                .CanFilter()
                .HasName("name");

            mapper.Property<UserDetailsResponse>(p => p.FullName)
                .CanFilter()
                .HasName("name");

            mapper.Property<UserDetailsResponse>(p => p.Email)
                .CanSort()
                .CanFilter()
                .HasName("gmail");

            mapper.Property<UserDetailsResponse>(p => p.Department)
                .CanSort()
                .CanFilter();

            mapper.Property<UserDetailsResponse>(p => p.DateOfBirth)
                .CanSort()
                .CanFilter()
                .HasName("birth_date");

            mapper.Property<UserDetailsResponse>(p => p.Role)
                .CanSort()
                .CanFilter();
        }
    }
}