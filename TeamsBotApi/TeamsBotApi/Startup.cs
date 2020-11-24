using System.Linq;
using BuildSystem;
using BuildSystem.Api;
using BuildSystem.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TeamsBotApi.Data;
using TeamsBotApi.Services;

namespace TeamsBotApi
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
            services.AddDbContext<NotificationDbContext>(options => options.UseInMemoryDatabase("Notifications"), ServiceLifetime.Singleton);
            services.AddDbContext<BuildDbContext>(options => options.UseInMemoryDatabase("Builds"), ServiceLifetime.Singleton);

            services.AddControllers();

            services.AddTransient<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();
            services.AddTransient<IBot, BotHandler>();

            services.AddSingleton(s => new BuildFacade(s.GetService<IBuildRepository>()));
            services.AddSingleton<IBuildRepository, BuildRepository>();
            services.AddSingleton<NotificationService>();
            services.AddSingleton<MetricsService>();
            services.AddTransient<CommandParser>();
            services.AddTransient<CommandService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();
            //app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            SeedDatabase(app);

            // TODO: Move to hosted service.
            app.ApplicationServices.GetService<MetricsService>();
        }

        private static void SeedDatabase(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetService<BuildDbContext>();
            context.Database.EnsureCreated();

            if(context.Metadata.Any())
            {
                return;
            }

            context.Metadata.AddRange(BuildDefinitions.Definitions.Select(bd => new BuildMetadata(bd.Name)));
            context.SaveChanges();
        }
    }
}