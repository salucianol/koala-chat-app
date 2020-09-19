using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using KoalaChatApp.Infrastructure.Data;
using KoalaChatApp.Infrastructure.Models;
using KoalaChatApp.Web.Hubs;
using KoalaChatApp.Infrastructure.Interfaces;
using KoalaChatApp.Infrastructure.Helpers;
using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.Infrastructure.Services;
using KoalaChatApp.ApplicationCore.Specifications;
using KoalaChatApp.ApplicationCore.Entities;
using MediatR;
using KoalaChatApp.Infrastructure.Handlers;
using RabbitMQ.Client;
using KoalaChatApp.Infrastructure.Configurations;
using System.Reflection;
using KoalaChatApp.Web.Services;

namespace KoalaChatApp.Web {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<KoalaChatDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("LocalDB")));
            services.AddDbContext<KoalaChatIdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("LocalDB")));

            services.AddIdentity<ChatUser, IdentityRole<Guid>>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<KoalaChatIdentityDbContext>();

            RabbitMqConfigurations rabbitConfigurations = new RabbitMqConfigurations();
            Configuration.GetSection("RabbitMqConfigurations").Bind(rabbitConfigurations);
            services.AddSingleton<RabbitMQ.Client.IConnectionFactory>(new ConnectionFactory {
                HostName = rabbitConfigurations.Hostname,
                Port = rabbitConfigurations.Port,
                UserName = rabbitConfigurations.Username,
                Password = rabbitConfigurations.Password
            });

            services.AddSingleton<ICommandsHelper, CommandsHelper>();
            services.AddSingleton<IMessageQueue, MessageQueue>();
            services.AddSingleton<IChatHubService, ChatHubService>();
            services.AddScoped<IMessageParser, MessageParser>();
            services.AddScoped<IRepository<ChatRoom>, ChatRoomRepository>();
            services.AddScoped<IRepository<ChatUser>, UserRepository>();
            services.AddScoped<IChatRoomService, ChatRoomService>();
            services.AddScoped<ISpecification<ChatRoom>, ChatRoomSpecification>();
            services.AddScoped<IRequestHandler<ChatMessageRequestModel, bool>, ProcessMessageSentHandler>();

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSignalR();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = Configuration.GetSection("UserSettings").GetValue<byte>("PasswordRequiredLength");
                options.Password.RequiredUniqueChars = 1;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Configuration.GetSection("UserSettings").GetValue<byte>("LockoutTimeout"));
                options.Lockout.MaxFailedAccessAttempts = Configuration.GetSection("UserSettings").GetValue<byte>("MaxRetriesLogin");
                options.Lockout.AllowedForNewUsers = true;
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=ChatUser}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<KoalaChatHub>("/koalachat");
            });
        }
    }
}
