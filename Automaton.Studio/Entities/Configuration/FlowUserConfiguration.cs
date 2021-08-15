using Automaton.Studio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Automaton.Studio.Entities.Configuration
{
    public class FlowUserConfiguration : IEntityTypeConfiguration<FlowUser>
    {
        public void Configure(EntityTypeBuilder<FlowUser> builder)
        {
            builder.HasKey(e => new { e.FlowId, e.UserId });
        }
    }
}