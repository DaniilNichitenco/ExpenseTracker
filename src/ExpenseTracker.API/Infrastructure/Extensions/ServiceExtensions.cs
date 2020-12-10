using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using ExpenseTracker.API.Infrastructure.Configurations;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.API.Repositories.Implementations;
using Microsoft.AspNetCore.Authorization;
using ExpenseTracker.API.Authorization.IEnumerableUserDtoAuthHandler;
using ExpenseTracker.API.Authorization.BaseEntityAuthHandler;
using ExpenseTracker.API.Authorization.IEnumerableBaseEntityAuthHandler;
using ExpenseTracker.API.Authorization.IEnumerableExpenseDtoAuthHandler;
using ExpenseTracker.API.Authorization.ExpenseDtoAuthHandler;

namespace ExpenseTracker.API.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserInfoRepository, UserInfoRepository>();
            services.AddScoped<IPurseRepository, PurseRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();

            return services;
        }

        public static IServiceCollection AddAuthorizationHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, IEnumerableUserDtoAdministratorsAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, BaseEntityAdministratorsAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, IEnumerableBaseEntityAdministratorsAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, IEnumerableExpenseDtoAdministratorsAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ExpenseDtoAdministratorsAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, BaseEntityIsOwnerAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, IEnumerableBaseEntityIsOwnerAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, IEnumerableExpenseDtoIsOwnerAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ExpenseDtoIsOwnerAuthorizationHandler>();

            return services;
        }
        public static void AddJwtAuthentication(this IServiceCollection services, AuthOptions authOptions)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = authOptions.Audience,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                };
            });
        }
        public static AuthOptions ConfigureAuthOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var authOptionsConfigurationSection = configuration.GetSection("AuthOptions");
            services.Configure<AuthOptions>(authOptionsConfigurationSection);
            var authOptions = authOptionsConfigurationSection.Get<AuthOptions>();
            return authOptions;
        }
    }
}
