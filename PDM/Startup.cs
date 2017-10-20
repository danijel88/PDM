using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PDM.Data;
using PDM.Models;
using PDM.Models.Repository;
using PDM.Services;
using PDM.ViewModels;

namespace PDM
{
    public class Startup
    {
        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;


            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            Configuration = builder.Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            if (_env.IsDevelopment())
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }
            else if (_env.IsProduction())
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }


            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                //config.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<DbInitializer>();


            services.AddMvc();

            services.Configure<AuthMessageSenderOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            DbInitializer dbInitializer)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<ItemTypeViewModel, ItemType>().ReverseMap();
                config.CreateMap<MachineTypeViewModel, MachineType>().ReverseMap();
                config.CreateMap<ItemViewModel, Item>().ReverseMap();
                config.CreateMap<PdmViewModel, Pdm>().ReverseMap();
                config.CreateMap<ItemImageViewModel, ItemImage>().ReverseMap();
                config.CreateMap<ItemHistViewModel, ItemHist>().ReverseMap();
            });

            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

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

            dbInitializer.Initialize().Wait();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //new UserRoleSeed(app.ApplicationServices.GetService<RoleManager<IdentityRole>>()).Seed();
        }
    }
}
