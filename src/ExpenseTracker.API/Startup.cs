using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Domain.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Reflection;
using ExpenseTracker.API.Infrastructure.Extensions;
using ExpenseTracker.API.Infrastructure.Middlewares;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.API.Repositories.Implementations;

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
            services.AddDbContext<ExpenseTrackerDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ExpenseTrackerConnection")));

            services.AddIdentity<User, Role>(options =>
                options.Password.RequiredLength = 8)
                .AddEntityFrameworkStores<ExpenseTrackerDbContext>();

            var authOptions = services.ConfigureAuthOptions(Configuration);
            services.AddJwtAuthentication(authOptions);

            //services.AddControllers(options =>
            //    options.Filters.Add(new AuthorizeFilter()));
            services.AddControllers();

            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IOccasionRepository, OccasionRepository>();
            services.AddScoped<IPurseRepository, PurseRepository>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
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

            app.UseCors(configurePolicy => configurePolicy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            //app.UseHttpsRedirection();
            Console.WriteLine("conf");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
