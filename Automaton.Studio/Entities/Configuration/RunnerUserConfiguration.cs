using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Automaton.Studio.Entities.Configuration
{
    public class RunnerUserConfiguration : IEntityTypeConfiguration<RunnerUser>
    {
        public void Configure(EntityTypeBuilder<RunnerUser> builder)
        {
            builder.HasKey(e => new { e.RunnerId, e.UserId });
        }
    }
}