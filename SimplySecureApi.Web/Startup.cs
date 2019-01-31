using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SimplySecureApi.Data.DataAccessLayer.Boots;
using SimplySecureApi.Data.DataAccessLayer.Modules;
using SimplySecureApi.Data.DataAccessLayer.ModuleEvents;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Initialization;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Static;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using SimplySecureApi.Data.DataAccessLayer.Authentication;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.DataAccessLayer.LocationUsers;
using SimplySecureApi.Data.DataAccessLayer.PushNotificationTokens;
using SimplySecureApi.Data.Services.Messaging;
using SimplySecureApi.Web.MiddleWare;
using TokenOptions = SimplySecureApi.Data.Models.Authentication.TokenOptions;

namespace SimplySecureApi.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration
        {
            get;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SimplySecureDataContext>();

            services.AddScoped<IModuleEventRepository, ModuleEventRepository>();

            services.AddScoped<IBootRepository, BootRepository>();

            services.AddScoped<IPushNotificationTokensRepository, PushNotificationTokensRepository>();

            services.AddScoped<IModuleRepository, ModuleRepository>();

            services.AddScoped<IMessagingService, MessagingService>();

            services.AddScoped<ILocationRepository, LocationRepository>();

            services.AddScoped<ILocationUsersRepository, LocationUsersRepository>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddCors(o => o.AddPolicy("MyPolicy", corsBuilder =>
            {
                corsBuilder.WithOrigins("http://localhost:4200").WithOrigins("https://testzone.kaluba.tech")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequiredLength = 6;
                option.Password.RequiredUniqueChars = 0;
                option.Password.RequireLowercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<SimplySecureDataContext>()
              .AddDefaultTokenProviders();



            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["TokenOptions:Issuer"],
                        ValidAudience = Configuration["TokenOptions:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApplicationConfig.JwtTokenKey)),
                    };
                });

            services.AddMvc( )
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization(options => options.AddPolicy("Trusted", policy => policy.RequireClaim("DefaultUserClaim", "DefaultUserAuthorization")));

            services.AddOptions();

            services.Configure<TokenOptions>(Configuration.GetSection("TokenOptions"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseBrowserLink();
                    app.UseDatabaseErrorPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }

                app.UseCors("MyPolicy");

                app.UseMiddleware<MaintainCorsHeadersMiddleware>();

                app.UseStaticFiles();

                app.UseAuthentication();

                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });

                DataContextInitializer.Seed(app.ApplicationServices).Wait();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}