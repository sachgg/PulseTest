using Application.Clients;

namespace Application;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
    }
}