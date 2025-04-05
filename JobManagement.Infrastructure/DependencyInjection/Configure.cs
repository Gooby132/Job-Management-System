using JobManagement.Domain.JobManagers.Entities.Abstractions;
using JobManagement.Domain.JobManagers.Services;
using JobManagement.Infrastructure.Authorization.JwtAuthorization.DependencyInjection;
using JobManagement.Infrastructure.ConcreteJobs.InMemoryJobExecutionBag;
using JobManagement.Infrastructure.ConcreteJobs.JobProviderService;
using JobManagement.Infrastructure.ConcreteJobs.Jobs;
using JobManagement.Infrastructure.JobBackgroundService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobManagement.Infrastructure.DependencyInjection;

public static class Configure
{

    public const string Key = "Infrastructure";

    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        config = config.GetRequiredSection(Key);

        services.Configure<JobServiceOptions>(
            config.GetRequiredSection(JobServiceOptions.Key));

        services.ConfigureJwtAuthorization(config);
        services.AddSwaggerGen(c => c.AddSwaggerGenUsers(config));

        services.AddSingleton<IExecutionProvider, LocalExecutionProvider>();
        services.AddSingleton<IJobExecutionBag, JobExecutionBag>();

        services.AddHostedService<JobManagerService>();

        return services;
    }

    public static IServiceProvider SeedLocalExecutables(this IServiceProvider serviceProvider)
    {
        var jobProvider = serviceProvider.GetRequiredService<IExecutionProvider>() as LocalExecutionProvider;

        jobProvider!.AddAvailableJobs<SuccessfulTimerJob>(SuccessfulTimerJob.Name);

        return serviceProvider;
    }

}
