using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Test.MVC
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
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookie";
                options.DefaultChallengeScheme = "OIDC";
            })
            .AddCookie("Cookie")
            .AddOpenIdConnect("OIDC", options =>
            {
                options.Authority = "https://localhost:5000";
                options.ClientId = "oidc";
                options.ClientSecret = "SuperSecretPassword";

                options.ResponseType = "code";
                options.UsePkce = true;
                options.ResponseMode = "query";

                // options.CallbackPath = "/signin-oidc"; // default redirect URI

                // options.Scope.Add("oidc"); // default scope
                // options.Scope.Add("profile"); // default scope
                options.Scope.Add("test.api.read");

                options.SaveTokens = true;
            });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
