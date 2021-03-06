﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReSharpersCooperation.Data;
using ReSharpersCooperation.Models;
using ReSharpersCooperation.Services;
using Microsoft.AspNetCore.Http;
using ReSharpersCooperation.Models.CartRepository;
using ReSharpersCooperation.Models.ProductVIewModels;
using ReSharpersCooperation.Models.Category;

namespace ReSharpersCooperation
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ProductRepository>();
            services.AddTransient<CartRepository>();
            services.AddTransient<CartItemRepository>();
            services.AddTransient<CategoryRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHostingEnvironment>(Environment);
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSession();
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
                name: "pagination",
                template: "Products/{productPage}",
                defaults: new { Controller = "Product", Action = "List" }
                );
                routes.MapRoute(
                    name: "cart",
                    template: "Cart",
                    defaults: new { Controller = "Cart", Action = "Index"}
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Product}/{action=List}");
            });
        }
    }
}
