using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DataATest.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Localization.SqlLocalizer.DbStringLocalizer;
namespace DataATest
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private readonly IHostingEnvironment _env;
        public Startup(IConfiguration config, IHostingEnvironment env)
        {
            _env = env;


        }

        public IConfigurationRoot Configuration { get; set; }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<LocalizationModelContext>(opt => opt.UseInMemoryDatabase("Test"),
                ServiceLifetime.Singleton,
                ServiceLifetime.Singleton);

            var useTypeFullNames = false;
            var useOnlyPropertyNames = true;
            var returnOnlyKeyIfNotFound = true;

            services.AddSqlLocalization(options => options.UseSettings(
              useTypeFullNames,
              useOnlyPropertyNames,
              returnOnlyKeyIfNotFound,
              false));

            services.Configure<RequestLocalizationOptions>(
                  options =>
                  {
                      var supportedCultures = new List<CultureInfo>
                          {
                                            new CultureInfo("en-US"),
                                            new CultureInfo("de-CH"),
                                            new CultureInfo("fr-CH"),
                                            new CultureInfo("it-CH")
                          };

                      options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                      options.SupportedCultures = supportedCultures;
                      options.SupportedUICultures = supportedCultures;
                  });

            services.AddTransient<DataSeeder>();

            //https://offering.solutions/blog/articles/2017/02/07/difference-between-addmvc-addmvcore/
            services.AddMvc(opt =>
            {
                if (_env.IsProduction() && Configuration["DisableSSL"] != "true")
                {
                    opt.Filters.Add(new RequireHttpsAttribute());
                }
            })
            .AddViewLocalization()
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(SharedResource));
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }
            else
            {

                app.UseExceptionHandler("/error");
            }

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);


           
            app.UseStaticFiles();
            app.UseMvc(ConfigureRoutes);

            if (env.IsDevelopment())
            {
                //Seed the database
                using (var scope = app.ApplicationServices.CreateScope())
                {

                    var seeder = scope.ServiceProvider.GetService<DataSeeder>();
                    seeder.Seed().Wait();

                }
            }
        }
        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            // /Home/Index
            // the = after the controller etc is the default used if no controller or action is found
            //This is known as convection based routing
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}

