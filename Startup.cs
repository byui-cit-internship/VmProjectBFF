using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using VmProjectBFF.Handlers;
using VmProjectBFF.Services;

namespace VmProjectBFF
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
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

            services.AddScoped<IBackendHttpClient, BackendHttpClient>();
            services.AddScoped<ICanvasHttpClient, CanvasHttpClient>();
            services.AddScoped<IVCenterHttpClient, VCenterHttpClient>();
            services.AddScoped<IBackendRepository, BackendRepository>();
            services.AddScoped<ICanvasRepository, CanvasRepository>();
            services.AddScoped<IVCenterRepository, VCenterRepository>();
            services.AddScoped<IAuthorization, Authorization>();
            services.AddSingleton<IEmailClient, EmailClient>();

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
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IActionDescriptorCollectionProvider actionProvider)
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
            Logger.LogInformation("Available routes:");
            IEnumerable<ActionDescriptor> routes = actionProvider.ActionDescriptors.Items.Where(x => x.AttributeRouteInfo != null);
            foreach (ActionDescriptor route in routes)
            {
                Logger.LogInformation($"{route.AttributeRouteInfo.Template}");
            }
            Logger.LogInformation("Application configured successfully");
        }
    }
}
