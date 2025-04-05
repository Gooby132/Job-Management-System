
using JobManagement.Domain.Common;
using JobManagement.Domain.Users;
using JobManagement.Domain.Users.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JobManagement.Persistence.Users;

public static class SeedUsers
{

    public const string Key = "SeedUsers";

    public static IServiceCollection ConfigureSeed(
        this IServiceCollection services,
        IConfiguration config)
    {
        config = config.GetRequiredSection(Key);

        services.Configure<UserSeedOptions>(
            config.GetRequiredSection(UserSeedOptions.Key));

        return services;
    }

    public static async Task<IServiceProvider> SeedAdmin(
        this IServiceProvider serviceProvider,
        CancellationToken token = default)
    {
        using var scope = serviceProvider.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var userSeed = scope.ServiceProvider.GetRequiredService<IOptions<UserSeedOptions>>()
            .Value;

        var userName = UserName.Create(userSeed.UserName);
        var userPassword = UserPassword.Create(userSeed.Password);

        if (userName.IsFailed || userPassword.IsFailed)
            throw new InvalidOperationException("user seed was not configured correctly");

        var exists = await userRepository.GetUserByName(userName.Value, token);

        if (exists.IsFailed)
            throw new InvalidOperationException("admin exists failed");

        if (exists.Value is not null)
            return serviceProvider;

        var user = User.Create(userName.Value, userPassword.Value, UserRole.Operator);

        var persist = await userRepository.Persist(user.Value, token);

        if (persist.IsFailed)
            throw new InvalidOperationException("persisting user failed");

        var uow = await unitOfWork.Commit();

        if (uow.IsFailed)
            throw new InvalidOperationException("committing user failed");

        return serviceProvider;
    }
}
