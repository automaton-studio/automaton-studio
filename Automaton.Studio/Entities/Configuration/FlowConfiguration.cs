using Automaton.Studio.Entities;
using Elsa.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Automaton.Studio.Entities.Configuration
{
    public class FlowConfiguration : IEntityTypeConfiguration<Flow>
    {
        public void Configure(EntityTypeBuilder<Flow> builder)
        {
            builder.Property(e => e.Name).IsRequired().HasMaxLength(128);
            builder.Property(e => e.StartupWorkflowId).IsRequired().HasMaxLength(128);
        }
    }
}