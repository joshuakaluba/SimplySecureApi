using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SimplySecureApi.Data.DataAccessLayer.Boots;
using SimplySecureApi.Data.DataAccessLayer.Modules;
using SimplySecureApi.Data.DataAccessLayer.StateChanges;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Initialization;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Static;
using System.Text;
using TokenOptions = SimplySecureApi.Data.Models.TokenOptions;

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

            services.AddScoped<IStateChangesRepository, StateChangesRepository>();

            services.AddScoped<IBootRepository, BootRepository>();

            services.AddScoped<IModuleRepository, ModuleRepository>();

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

            services.AddMvc();

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