using Automaton.Studio.Server.Entities;
using Common.Authentication;
using Common.EF;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Automaton.Studio.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IDataContext
    {
        private IDbContextTransaction _transaction;

        public virtual DbSet<RefreshToken<Guid>> RefreshTokens { get; set; }
        public virtual DbSet<Flow> Flows { get; set; }
        public virtual DbSet<FlowUser> FlowUsers { get; set; }
        public virtual DbSet<Runner> Runners { get; set; }
        public virtual DbSet<RunnerUser> RunnerUsers { get; set; }
        public virtual DbSet<LogMessage> Logs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flow>(entity =>
            {
                entity.HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<FlowUser>(entity =>
            {
                entity.HasKey(e => new { e.FlowId, e.UserId });
                entity.HasIndex(e => e.FlowId, "IX_FlowUser_FlowId");
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.FlowId).IsRequired();
            });

            modelBuilder.Entity<Runner>(entity =>
            {
                entity.HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<RunnerUser>(entity =>
            {
                entity.HasKey(e => new { e.RunnerId, e.UserId });
                entity.HasIndex(e => e.RunnerId, "IX_RunnerUser_RunnerId");
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.RunnerId).IsRequired();
            });

            modelBuilder.Entity<RefreshToken<Guid>>()
                .ToTable("RefreshTokens");

            modelBuilder.Entity<RefreshToken<Guid>>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<RefreshToken<Guid>>().Property(x => x.UserId);

            modelBuilder.Entity<RefreshToken<Guid>>()
                .Property(s => s.Id)
                .HasDefaultValueSql("NEWID()")
                .IsRequired();

            modelBuilder.Entity<RefreshToken<Guid>>()
                .Property(s => s.Token)
                .IsRequired();

            modelBuilder.Entity<RefreshToken<Guid>>()
                .Property(s => s.RevokedAt)
                .IsRequired(false);

            modelBuilder.Entity<RefreshToken<Guid>>()
                .Property(s => s.CreatedAt)
                .IsRequired()
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<RefreshToken<Guid>>()
                .Property(s => s.Expires)
                .IsRequired();
        }

        public void BeginTransaction()
        {
            _transaction = Database.BeginTransaction();
        }

        public int Commit()
        {
            try
            {
                var saveChanges = SaveChanges();
                _transaction.Commit();
                return saveChanges;
            }
            finally
            {
                _transaction.Dispose();
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }

        public Task<int> CommitAsync()
        {
            try
            {
                var saveChangesAsync = SaveChangesAsync();
                _transaction.Commit();
                return saveChangesAsync;
            }
            finally
            {
                _transaction.Dispose();
            }
        }
    }
}