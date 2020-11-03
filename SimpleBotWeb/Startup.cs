using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.Factories;
using SimpleBotWeb.Models.Values;

namespace SimpleBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var settings = configuration.GetSection("AppSettings");
            AppConfiguration.ConnectionString = settings["ConnectionString"];
            AppConfiguration.BaseUrl = settings["BaseUrl"];

            using (var dc = DatacontextFactory.GetDatabase())
            {
                var ch = new ConfigurationHelper(dc);
                AppConfiguration.SiteConfiguration = ch.GetConfiguration();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                    {
                        options.AccessDeniedPath = "/Account/AccessDenied";
                        options.LoginPath = "/Account/Login";
                        options.LogoutPath = "/Account/Logout";
                    }
                );
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Runs matching. An endpoint is selected and set on the HttpContext if a match is found.
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Executes the endpoint that was selected by routing.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "TinyUrlConvert",
                    pattern: "TinyUrl/Convert/{id}",
                    defaults: new { controller = "TinyUrl", action = "Convert" });

                endpoints.MapControllerRoute(name: "TinyUrlConvertAll",
                    pattern: "TinyUrl/ConvertAll",
                    defaults: new { controller = "TinyUrl", action = "ConvertAll" });

                endpoints.MapControllerRoute(name: "TinyUrlRedirect",
                    pattern: "TinyUrl/{id}",
                    defaults: new { controller = "TinyUrl", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
