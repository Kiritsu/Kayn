using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Kayn.Configurations;
using Kayn.Core.Extensions;
using Kayn.Core.Interfaces;
using Kayn.Data.Configurations;
using Kayn.Data.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Kayn
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var path = Environment.GetEnvironmentVariable("KAYN_HOME") ?? "kayn.json";

            var cfg = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .AddJsonFile(path, false)
                .Build();

            Configuration = cfg;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DiscordConfiguration>(Configuration.GetSection("Discord"))
                .AddSingleton<IDiscordConfiguration, DiscordConfiguration>(x =>
                {
                    var configuration = x.GetRequiredService<IOptions<DiscordConfiguration>>();
                    return configuration.Value;
                });

            services.Configure<DefaultDatabaseConfiguration>(Configuration.GetSection("Database"))
                .AddSingleton<IDatabaseConfiguration, DefaultDatabaseConfiguration>(x =>
                {
                    var configuration = x.GetRequiredService<IOptions<DefaultDatabaseConfiguration>>();
                    return configuration.Value;
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession(options =>
            {
                options.Cookie.Name = "kayn";
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });

            services.AddAuthentication(options =>
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/signin";
                    options.LogoutPath = "/signout";
                })
                .AddDiscord(options =>
                {
                    options.ClientId = Configuration["Discord:ClientId"];
                    options.ClientSecret = Configuration["Discord:ClientSecret"];

                    options.Scope.Add("identify");
                    options.Scope.Add("email");
                    options.Scope.Add("connections");
                    options.Scope.Add("guilds");
                    options.SaveTokens = true;

                    options.Events = new OAuthEvents
                    {
                        OnTicketReceived = ctx =>
                        {
                            var claimsIdentity = (ClaimsIdentity) ctx.Principal!.Identity;
                            var token = ctx.Properties.Items.FirstOrDefault(p => p.Key == ".Token.access_token").Value;

                            claimsIdentity!.AddClaim(new Claim("BearerToken", token!));

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddControllersWithViews();
            services.AddPostgresqlDatabaseContext();
            services.AddDiscordService();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}