using JobManagement.Domain.Common;
using JobManagement.Domain.JobManagers;
using JobManagement.Persistence.Context;
using JobManagement.Persistence.JobManagers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobManagement.Persistence.DependencyInjection;

public static class Configure
{

    public const string Key = "Persistence";

    public static IServiceCollection ConfigurePersistence(this IServiceCollection services, IConfiguration config)
    {
        //config = config.GetRequiredSection(Key);

        services.AddDbContext<ApplicationContext>();
        services.AddTransient<IJobManagerRepository, JobManagerRepository>();
        services.AddTransient<IUnitOfWork, ApplicationUnitOfWork>();

        return services;
    }

}
