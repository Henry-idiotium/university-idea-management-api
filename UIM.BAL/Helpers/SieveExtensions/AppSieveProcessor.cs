using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using UIM.BAL.Helpers.SieveExtensions.Configurations;

namespace UIM.BAL.Helpers.SieveExtensions
{
    public class AppSieveProcessor : SieveProcessor
    {
        public AppSieveProcessor(IOptions<SieveOptions> options) : base(options) { }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            return mapper
                .ApplyConfiguration<SieveConfigurationForUser>();
        }
    }
}