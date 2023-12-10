using Infrastructure.Clients;

namespace Infrastructure;

public static class ServiceExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDatabaseStuff();
        services.AddRepositories();
    }

    private static void AddDatabaseStuff(this IServiceCollection services)
    {
        services.AddDbContext<ClientContext>();
        services.AddDatabaseDeveloperPageExceptionFilter();
    }

    private static void AddRepositories(this IServiceCollection services) 
    { 
        services.AddScoped<IClientRepository, ClientRepository>();
    }
}