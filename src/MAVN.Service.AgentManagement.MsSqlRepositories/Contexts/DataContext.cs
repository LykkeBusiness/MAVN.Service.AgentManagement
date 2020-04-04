using System.Data.Common;
using JetBrains.Annotations;
using Lykke.Common.MsSql;
using MAVN.Service.AgentManagement.Domain.Entities;
using MAVN.Service.AgentManagement.Domain.Entities.Agents;
using MAVN.Service.AgentManagement.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MAVN.Service.AgentManagement.MsSqlRepositories.Contexts
{
    public class DataContext : MsSqlContext
    {
        private const string Schema = "agent_management";

        internal DbSet<AgentEntity> Agents { get; set; }

        internal DbSet<TokensRequirementEntity> TokensRequirement { get; set; }

        internal DbSet<ImageEntity> Images { get; set; }

        // C-tor for EF migrations
        [UsedImplicitly]
        public DataContext()
            : base(Schema)
        {
        }

        public DataContext(string connectionString, bool isTraceEnabled)
            : base(Schema, connectionString, isTraceEnabled)
        {
        }

        public DataContext(DbConnection dbConnection)
            : base(Schema, dbConnection)
        {
        }

        protected override void OnLykkeModelCreating(ModelBuilder modelBuilder)
        {
            // Agent
            modelBuilder.Entity<AgentEntity>()
                .Property(o => o.Status)
                .HasConversion(new EnumToNumberConverter<AgentStatus, short>());

            // Image
            modelBuilder.Entity<ImageEntity>()
                .HasOne<AgentEntity>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .HasConstraintName("fk_images_agents");

            modelBuilder.Entity<ImageEntity>()
                .Property(o => o.DocumentType)
                .HasConversion(new EnumToNumberConverter<DocumentType, short>());
        }
    }
}
