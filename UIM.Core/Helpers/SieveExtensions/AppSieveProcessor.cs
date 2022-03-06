using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using UIM.Core.Helpers.SieveExtensions.Configurations;

namespace UIM.Core.Helpers.SieveExtensions
{
    public class AppSieveProcessor : SieveProcessor
    {
        public AppSieveProcessor(IOptions<SieveOptions> options) : base(options) { }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            return mapper
                .ApplyConfiguration<UserSieveConfig>()
                .ApplyConfiguration<SubmissionSieveConfig>();
        }
    }
}