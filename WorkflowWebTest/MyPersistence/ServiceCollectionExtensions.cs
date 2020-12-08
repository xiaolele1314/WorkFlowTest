using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Persistence.EntityFramework.Services;

namespace WorkflowWebTest.Test
{
    public static class ServiceCollectionExtensions
    {
        public static WorkflowOptions UseMySQL(this WorkflowOptions options, string connectionString, bool canCreateDB = true, bool canMigrateDB = true, Action<MySqlDbContextOptionsBuilder> mysqlOptionsAction = null)
        {
            options.UsePersistence(sp => new MyEntityFrameworkPersistenceProvider(new TestContextFactory(connectionString, mysqlOptionsAction), canCreateDB, canMigrateDB));
            options.Services.AddTransient<IWorkflowPurger>(sp => new WorkflowPurger(new TestContextFactory(connectionString, mysqlOptionsAction)));
            return options;
        }
    }
}
