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
        services.AddSwaggerGen();

        services.AddCors(builder =>
        {
            builder.AddDefaultPolicy(p =>
            {
                p.AllowAnyHeader();
                p.AllowAnyMethod();
                p.AllowAnyOrigin();
            });
        });

        services.AddSignalR();

        services.AddTransient<
            IJobManagerNotificationService,
            JobManagerSignalRNotificationService>();

        return services;
    }

}
