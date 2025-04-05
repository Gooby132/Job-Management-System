using JobManagement.Domain.Common;
using JobManagement.Domain.JobManagers;
using JobManagement.Domain.Users;
using JobManagement.Persistence.Context;
using JobManagement.Persistence.JobManagers;
using JobManagement.Persistence.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobManagement.Persistence.DependencyInjection;

public static class Configure
{

    public const string Key = "Persistence";

    public static IServiceCollection ConfigurePersistence(this IServiceCollection services, IConfiguration config)
    {
        config = config.GetRequiredSection(Key);

        services.ConfigureSeed(config);
        services.AddDbContext<ApplicationContext>();
        services.AddTransient<IJobManagerRepository, JobManagerRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUnitOfWork, ApplicationUnitOfWork>();

        return services;
    }

    public static async Task<IServiceProvider> RunPersistence(this IServiceProvider services, CancellationToken token)
    {

        await services.SeedAdmin(token);

        return services;
    }

}
