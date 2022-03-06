using Sieve.Services;
using UIM.Core.Models.Entities;

namespace UIM.Core.Helpers.SieveExtensions.Configurations
{
    public class SubmissionSieveConfig : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapp)
        {
            mapp.Property<Idea>(_ => _.Title).CanSort().CanFilter();
            mapp.Property<Idea>(_ => _.Description).CanSort().CanFilter();
            mapp.Property<Idea>(_ => _.Content).CanSort().CanFilter();
            mapp.Property<Idea>(_ => _.CreatedBy).CanSort().CanSort();
            mapp.Property<Idea>(_ => _.ModifiedBy).CanSort().CanSort();
            mapp.Property<Idea>(_ => _.CreatedDate).CanSort().CanSort();
            mapp.Property<Idea>(_ => _.ModifiedDate).CanSort().CanSort();
        }
    }
}