using Automaton.Studio.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Automaton.Studio
{
    public partial class AutomatonDbContext : DbContext
    {
        public AutomatonDbContext()
        {
        }

        public AutomatonDbContext(DbContextOptions<AutomatonDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Bookmark> Bookmarks { get; set; }
        public virtual DbSet<WorkflowDefinition> WorkflowDefinitions { get; set; }
        public virtual DbSet<WorkflowExecutionLogRecord> WorkflowExecutionLogRecords { get; set; }
        public virtual DbSet<WorkflowInstance> WorkflowInstances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Bookmark>(entity =>
            {
                entity.HasIndex(e => e.ActivityId, "IX_Bookmark_ActivityId");

                entity.HasIndex(e => e.ActivityType, "IX_Bookmark_ActivityType");

                entity.HasIndex(e => new { e.ActivityType, e.TenantId, e.Hash }, "IX_Bookmark_ActivityType_TenantId_Hash");

                entity.HasIndex(e => e.Hash, "IX_Bookmark_Hash");

                entity.HasIndex(e => e.TenantId, "IX_Bookmark_TenantId");

                entity.HasIndex(e => e.WorkflowInstanceId, "IX_Bookmark_WorkflowInstanceId");

                entity.Property(e => e.ActivityId).IsRequired();

                entity.Property(e => e.ActivityType).IsRequired();

                entity.Property(e => e.Hash).IsRequired();

                entity.Property(e => e.Model).IsRequired();

                entity.Property(e => e.ModelType).IsRequired();

                entity.Property(e => e.WorkflowInstanceId).IsRequired();
            });

            modelBuilder.Entity<WorkflowDefinition>(entity =>
            {
                entity.HasIndex(e => new { e.DefinitionId, e.Version }, "IX_WorkflowDefinition_DefinitionId_VersionId");

                entity.HasIndex(e => e.IsLatest, "IX_WorkflowDefinition_IsLatest");

                entity.HasIndex(e => e.IsPublished, "IX_WorkflowDefinition_IsPublished");

                entity.HasIndex(e => e.Name, "IX_WorkflowDefinition_Name");

                entity.HasIndex(e => e.TenantId, "IX_WorkflowDefinition_TenantId");

                entity.HasIndex(e => e.Version, "IX_WorkflowDefinition_Version");

                entity.Property(e => e.DefinitionId).IsRequired();
            });

            modelBuilder.Entity<WorkflowExecutionLogRecord>(entity =>
            {
                entity.Property(e => e.ActivityId).IsRequired();

                entity.Property(e => e.ActivityType).IsRequired();

                entity.Property(e => e.WorkflowInstanceId).IsRequired();
            });

            modelBuilder.Entity<WorkflowInstance>(entity =>
            {
                entity.HasIndex(e => e.ContextId, "IX_WorkflowInstance_ContextId");

                entity.HasIndex(e => e.ContextType, "IX_WorkflowInstance_ContextType");

                entity.HasIndex(e => e.CorrelationId, "IX_WorkflowInstance_CorrelationId");

                entity.HasIndex(e => e.CreatedAt, "IX_WorkflowInstance_CreatedAt");

                entity.HasIndex(e => e.DefinitionId, "IX_WorkflowInstance_DefinitionId");

                entity.HasIndex(e => e.FaultedAt, "IX_WorkflowInstance_FaultedAt");

                entity.HasIndex(e => e.FinishedAt, "IX_WorkflowInstance_FinishedAt");

                entity.HasIndex(e => e.LastExecutedAt, "IX_WorkflowInstance_LastExecutedAt");

                entity.HasIndex(e => e.Name, "IX_WorkflowInstance_Name");

                entity.HasIndex(e => e.TenantId, "IX_WorkflowInstance_TenantId");

                entity.HasIndex(e => e.WorkflowStatus, "IX_WorkflowInstance_WorkflowStatus");

                entity.HasIndex(e => new { e.WorkflowStatus, e.DefinitionId, e.Version }, "IX_WorkflowInstance_WorkflowStatus_DefinitionId_Version");

                entity.Property(e => e.DefinitionId).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
