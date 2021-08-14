using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Automaton.Studio.Entities.Configuration
{
    public class RunnerConfiguration : IEntityTypeConfiguration<Runner>
    {
        public void Configure(EntityTypeBuilder<Runner> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).HasMaxLength(128);
            builder.Property(e => e.ConnectionId).HasMaxLength(128);
        }
    }
}