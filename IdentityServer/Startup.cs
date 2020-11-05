using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace IdentityServer
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = configuration.GetConnectionString("IdentityServerDb");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                // SQL Database
                .AddOperationalStore(options =>
                    options.ConfigureDbContext =
                        builder => builder.UseSqlServer(
                            connectionString,
                            sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                .AddConfigurationStore(options =>
                    options.ConfigureDbContext =
                        builder => builder.UseSqlServer(
                            connectionString,
                            sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                // In-memory
                //.AddInMemoryClients(Clients.Get())
                //.AddInMemoryIdentityResources(Resources.GetIdentityResources())
                //.AddInMemoryApiResources(Resources.GetApiResources())
                //.AddInMemoryApiScopes(Resources.GetApiScopes())
                //.AddTestUsers(TestUsers.Get())
                .AddDeveloperSigningCredential();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
