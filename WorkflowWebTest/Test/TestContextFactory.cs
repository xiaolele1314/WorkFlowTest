using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Persistence.EntityFramework.Interfaces;
using WorkflowCore.Persistence.EntityFramework.Services;

namespace WorkflowWebTest.Test
{
    public class TestContextFactory : IWorkflowDbContextFactory
    {
        private readonly string _connectionString;
        private readonly Action<MySqlDbContextOptionsBuilder> _mysqlOptionsAction;

        public TestContextFactory(string connectionString, Action<MySqlDbContextOptionsBuilder> mysqlOptionsAction = null)
        {
            _connectionString = connectionString;
            _mysqlOptionsAction = mysqlOptionsAction;
            var a = new TestDbcontext(_connectionString, _mysqlOptionsAction);
        }

        public WorkflowDbContext Build()
        {
            return new TestDbcontext(_connectionString, _mysqlOptionsAction);
        }
    }
}
