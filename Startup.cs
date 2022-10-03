using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using vmProjectBFF.Handlers;
using System.Reflection;
using System.IO;
using vmProjectBFF.Services;

namespace vmProjectBFF
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            DotEnv.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Environment = env;
            MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public string MyAllowSpecificOrigins { get; }

        ILogger Logger { get; } = AppLogger.CreateLogger<Startup>();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // This helps to prevent the Reference Loop that is caused
            // when you reference a model inside another model, model like
            // enrollment which reference Course and User and both making a Reference back to enrollment

            services.AddControllers()
           .AddNewtonsoftJson(
               opts => opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
           );

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            // This is needed to register my Background service
            // UNCOMMENT LATER services.AddHostedService<BackgroundService1>();
            // Allow to use client Factory
            services.AddHttpClient();

            services.AddHttpContextAccessor();

            services.AddDistributedMemoryCache();

            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            // This allows for Cross-origin request Read more: https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0 
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:5501").AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials().AllowAnyMethod();
                                  });
            });
            // ******************CHNAGE IN FUTURE**********************************

            //this helps to connect the authentication for controllers request to the BasicAuthenticationHandler 
            services.AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            Logger.LogInformation("Services Configured correctly");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.AspNetCore.Mvc.Infrastructure.IActionDescriptorCollectionProvider actionProvider)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            // *******************CHNAGE SOON******************************
            

            // ***************CHNAGE SOON**************************
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            // This tell app that it will use authentication
            app.UseAuthentication();
            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            Console.WriteLine("Available routes:");
            var routes = actionProvider.ActionDescriptors.Items.Where(x => x.AttributeRouteInfo != null);
            foreach (var route in routes)
            {
                Console.WriteLine($"{route.AttributeRouteInfo.Template}");
            }
            Console.WriteLine("Application configured successfully");
        }
    }
}
