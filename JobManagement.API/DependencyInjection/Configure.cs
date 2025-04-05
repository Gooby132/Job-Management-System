using JobManagement.API.Services;
using JobManagement.Domain.JobManagers.Services;

namespace JobManagement.API.DependencyInjection;

public static class Configure
{
    public const string Key = "Application";

    public static IServiceCollection ConfigureApplication(
        this IServiceCollection services,
        IConfiguration config)
    {
        //config = config.GetRequiredSection(Key);

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddCors(builder =>
        {
            builder.AddDefaultPolicy(p =>
            {
                p.WithOrigins("http://localhost:5173");
                p.AllowAnyHeader();
                p.AllowAnyMethod();
                p.AllowCredentials();
            });
        });

        services.AddSignalR();

        services.AddTransient<
            IJobManagerNotificationService,
            JobManagerSignalRNotificationService>();

        return services;
    }

}
