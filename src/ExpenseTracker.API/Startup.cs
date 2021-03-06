using System;
using ExpenseTracker.Domain.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using System.Reflection;
using ExpenseTracker.API.Infrastructure.Extensions;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.API.Repositories.Implementations;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using ExpenseTracker.API.Authorization.BaseEntityAuthHandler;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ExpenseTracker.API.Authorization.IEnumerableBaseEntityAuthHandler;
using ExpenseTracker.API.Authorization.ExpenseDtoAuthHandler;
using ExpenseTracker.API.Authorization.IEnumerableExpenseDtoAuthHandler;
using ExpenseTracker.API.Authorization.IEnumerableUserDtoAuthHandler;

namespace ExpenseTracker.API
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
            //services.Configure<KestrelServerOptions>(
            //    Configuration.GetSection("Kestrel"));

            services.AddDbContext<ExpenseTrackerDbContext>(options => {

                options.UseSqlServer(Configuration.GetConnectionString("ExpenseTrackerConnection"));
                options.UseLazyLoadingProxies();
            });

            services.AddIdentity<User, Role>(options => {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<ExpenseTrackerDbContext>();

            var authOptions = services.ConfigureAuthOptions(Configuration);
            services.AddJwtAuthentication(authOptions);

            ConfigureSwagger(services);
            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                                
            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission", policy =>
                    policy.Requirements.Add(new OperationAuthorizationRequirement()));
            }
            );

            services.AddAuthorizationHandlers();

            services.AddRepositories();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Authorization/AccessDenied";
                options.Cookie.Name = "ExpenseTracker_Auth";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.LoginPath = "/Authorization/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
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
                app.UseErrorHandling();
                app.UseHsts();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseHttpsRedirection();

            app.UseCors(configurePolicy => configurePolicy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Swagger Expense Tracker API v1");
            });

            //app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            var contact = new OpenApiContact()
            {
                Name = "Daniil Nichitenco",
                Email = "daniilnikitenco@example.com",
                Url = new Uri("http://www.example.com")
            };

            var license = new OpenApiLicense()
            {
                Name = "My License",
                Url = new Uri("http://www.example.com")
            };

            var info = new OpenApiInfo()
            {
                Version = "v1",
                Title = "Swagger Expense Tracker API",
                Description = "Swagger Expense Tracker API Description",
                TermsOfService = new Uri("http://www.example.com"),
                Contact = contact,
                License = license
            };

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", info);
            });
        }
    }
}
