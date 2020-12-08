using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using WorkflowWebTest.Test;

namespace WorkflowWebTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<TestDbcontext>(opt => opt.UseMySql("server = service.byzan.cxist.com; userid = root; password = Iubang001!; database = Dev.fizz.Ord;",ServerVersion.AutoDetect("server = service.byzan.cxist.com; userid = root; password = Iubang001!; database = Dev.fizz.Ord;")));
            services.AddWorkflow(x => x.UseMySQL("Server=service.byzan.cxist.com;Database=workflow;User=root;Password=Iubang001!;"));
            services.AddControllers();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
