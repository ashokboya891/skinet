using API.Errors;
using Core.Interfaces;
using Infrastructure.Services;
using Infrastructure.Data;
using Infrastructure.Data.Migrations;
// using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Extensions
{
    public static class ApplicationservicesExtensions
    {
        public static IServiceCollection AddApplicartionservices(this IServiceCollection services
        , IConfiguration config)
        {
            services.AddSingleton<IResponseCacheService,ReponseCacheService>();

        
            services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));
                options.AbortOnConnectFail = false; // Allow the multiplexer to continue retrying
                options.ConnectRetry = 5; // Number of retri+es before giving up (adjust as needed)
                options.ConnectTimeout = 5000; // Connection timeout in milliseconds (adjust as needed)

                return ConnectionMultiplexer.Connect(options);

            });
            services.AddScoped<IBasketRepository,BasketRepository>();
            services.AddScoped<IPaymentService,PaymentService>();
            services.AddScoped<IOrderService,OrderService>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<ApiBehaviorOptions>(Options =>
                {
                    Options.InvalidModelStateResponseFactory = actionContext =>
            {
                    var errors = actionContext.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);

                };

                });
                services.AddCors(opt=>
                {
                opt.AddPolicy("CorsPolicy",policy=>
                    {
                        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                    });
                });
            return services;
        }
    }
}