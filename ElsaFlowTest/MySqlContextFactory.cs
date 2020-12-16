using Elsa.Persistence.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElsaFlowTest
{
    public class MySqlContextFactory : IDesignTimeDbContextFactory<MySqlContext>
    {
        public MySqlContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySqlContext>();
            var migrationAssembly = typeof(MySqlContext).Assembly.FullName;
            //var connectionString = Environment.GetEnvironmentVariable("EF_CONNECTIONSTRING");

            var connectionString = "server = service.byzan.cxist.com; userid = root; password = Iubang001!; database = elsa; ";
            if (connectionString == null)
                throw new InvalidOperationException("Set the EF_CONNECTIONSTRING environment variable to a valid MySQL connection string. E.g. SET EF_CONNECTIONSTRING=Server=localhost;Database=Elsa;User=sa;Password=Secret_password123!;");

            optionsBuilder.UseMySql(
                connectionString,
                x => x.MigrationsAssembly(migrationAssembly)
            );

            return new MySqlContext(optionsBuilder.Options);
        }
    }
}
