using Automaton.Studio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Automaton.Studio.Entities.Configuration
{
    public class FlowWorkflowConfiguration : IEntityTypeConfiguration<FlowWorkflow>
    {
        public void Configure(EntityTypeBuilder<FlowWorkflow> builder)
        {
            builder.HasKey(e => new { e.FlowId, e.WorkflowId });
        }
    }
}