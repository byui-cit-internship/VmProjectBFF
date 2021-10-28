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
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

using vmProjectBackend.Models;
using vmProjectBackend.DAL;
using Microsoft.AspNetCore.Authentication;
using vmProjectBackend.Handlers;

namespace vmProjectBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
           options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
           );
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader());

            });
            // ******************CHNAGE IN FUTURE**********************************
            services.AddControllers();

            //this helps to connect the authentication for controllers request to the BasicAuthenticationHandler 
            services.AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            // ********************ONLY FOR NOW USE****************************
            if (Environment.IsDevelopment())
            {

                string connectionString = Configuration.GetConnectionString("DevelopmentString");

                services.AddDbContext<VmContext>(opt =>
                                                 opt.UseSqlServer(connectionString));
            }

            if (Environment.IsProduction())
            {
                string connectionString = Configuration.GetConnectionString("ProductionString");

                services.AddDbContext<VmContext>(opt =>
                                                 opt.UseSqlServer(connectionString));

            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // *******************CHNAGE SOON******************************
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader());

            // ***************CHNAGE SOON**************************
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseSwagger();
                // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "vmProjectBackend v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // tell app that it will use autheication
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
