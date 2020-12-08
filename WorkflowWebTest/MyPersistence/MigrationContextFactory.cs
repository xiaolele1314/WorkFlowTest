using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkflowWebTest.Test
{
    public class MigrationContextFactory : IDesignTimeDbContextFactory<TestDbcontext>
    {
        public TestDbcontext CreateDbContext(string[] args)
        {
            return new TestDbcontext("Server=service.byzan.cxist.com;Database=workflow;User=root;Password=Iubang001!;");
        }
    }
}
