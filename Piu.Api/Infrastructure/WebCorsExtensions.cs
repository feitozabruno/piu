namespace Piu.Api.Infrastructure;

public static class CorsExtensions
{
    public static IServiceCollection AddWebCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("Web", policy =>
            {
                policy.WithOrigins("http://localhost:5173", "https://piu-vhfi.onrender.com", "https://piu-xi.vercel.app")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}
