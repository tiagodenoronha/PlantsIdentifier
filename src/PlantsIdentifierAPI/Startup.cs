using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlantsIdentifierAPI.Models;

namespace PlantsIdentifierAPI
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
            ///////////////////////// using a local .db file /////////////////////////
            services.AddDbContext<PlantsContext>(options => options.UseSqlite(Configuration.GetConnectionString("PlantContext")));

            /////////////////////////using a docker container with an SQL Server /////////////////////////
            //services.AddDbContext<PlantsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("PlantsSQLServer")));
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Trace);
            }
            else
            {
                app.UseHsts();
                //loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Information);
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
