using Microsoft.EntityFrameworkCore;

namespace Automaton.Persistence
{
    public partial class AutomatonDbContext : DbContext
    {
        public virtual DbSet<WorkflowDefinition> WorkflowDefinitions { get; set; }

        public AutomatonDbContext()
        {
        }

        public AutomatonDbContext(DbContextOptions<AutomatonDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkflowDefinition>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(256);
                entity.Property(e => e.Description).HasMaxLength(1024);
            });
        }
    }
}
