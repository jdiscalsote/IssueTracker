using IssueTracker.Models;
using IssueTracker.SystemServices;
using Microsoft.AspNetCore.Identity;

namespace IssueTracker
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
            services.AddMvc();
            services.Configure<StaticFileOptions>(options =>
            {
                options.OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Remove("Content-Type");

                    if (ctx.File.Name.EndsWith(".woff"))
                    {
                        ctx.Context.Response.Headers.Append("Content-Type", "application/x-font-woff");
                    }
                    else if (ctx.File.Name.EndsWith(".woff2"))
                    {
                        ctx.Context.Response.Headers.Append("Content-Type", "application/x-font-woff2");
                    }
                };
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddTransient<GeneralServices, GeneralServices>();
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust as needed
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddMvc().AddRazorPagesOptions(options => {
                options.Conventions.AddPageRoute("/Dashboard/Dashboard", "");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
