using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Persistence.EntityFramework.Models;
using WorkflowCore.Persistence.EntityFramework.Services;
using WorkflowCore.Persistence.MySQL;
using WorkflowWebTest.MyPersistence;

namespace WorkflowWebTest.Test
{
    public class TestDbcontext : WorkflowDbContext
    {
        private readonly string _connectionString;
        private readonly Action<MySqlDbContextOptionsBuilder> _mysqlOptionsAction;

        public TestDbcontext()
        {

        }
        public TestDbcontext(string connectionString, Action<MySqlDbContextOptionsBuilder> mysqlOptionsAction = null)
        {
            _connectionString = connectionString;
            _mysqlOptionsAction = mysqlOptionsAction;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(_connectionString, _mysqlOptionsAction);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var buider1 = modelBuilder.Entity<MyWorkflowDefination>();
            var buider2 = modelBuilder.Entity<MyWorkflowDefinationRelation>();
            ConfigureDefination(buider1);
            ConfigureDefinationRelation(buider2);
        }

        protected override void ConfigureSubscriptionStorage(EntityTypeBuilder<PersistedSubscription> builder)
        {
            builder.ToTable("Subscription");
            builder.Property(x => x.PersistenceId).ValueGeneratedOnAdd();
        }

        protected override void ConfigureWorkflowStorage(EntityTypeBuilder<PersistedWorkflow> builder)
        {
            builder.ToTable("Workflow");
            builder.Property(x => x.PersistenceId).ValueGeneratedOnAdd();
        }

        protected override void ConfigureExecutionPointerStorage(EntityTypeBuilder<PersistedExecutionPointer> builder)
        {
            builder.ToTable("ExecutionPointer");
            builder.Property(x => x.PersistenceId).ValueGeneratedOnAdd();
        }

        protected override void ConfigureExecutionErrorStorage(EntityTypeBuilder<PersistedExecutionError> builder)
        {
            builder.ToTable("ExecutionError");
            builder.Property(x => x.PersistenceId).ValueGeneratedOnAdd();
        }

        protected override void ConfigureExetensionAttributeStorage(EntityTypeBuilder<PersistedExtensionAttribute> builder)
        {
            builder.ToTable("ExtensionAttribute");
            builder.Property(x => x.PersistenceId).ValueGeneratedOnAdd();
        }

        protected override void ConfigureEventStorage(EntityTypeBuilder<PersistedEvent> builder)
        {
            builder.ToTable("Event");
            builder.Property(x => x.PersistenceId).ValueGeneratedOnAdd();
        }

        private void ConfigureDefination(EntityTypeBuilder<MyWorkflowDefination> builder)
        {
            builder.ToTable("Defination");
            builder.Property(x => x.PersistenceId).ValueGeneratedOnAdd();
        }

        private void ConfigureDefinationRelation(EntityTypeBuilder<MyWorkflowDefinationRelation> builder)
        {
            builder.ToTable("DefinationRelation");
            builder.Property(x => x.PersistenceId).ValueGeneratedOnAdd();
        }
    }
}
