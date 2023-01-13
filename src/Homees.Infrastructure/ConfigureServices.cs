using EventStore.Client;
using Homees.Application.Common.Interfaces;
using Homees.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Homees.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services
            .AddSingleton(new EventStoreClient(EventStoreClientSettings.Create("esdb://localhost:2113?tls=false")));

        services.AddSingleton<IStreamNameMapper, StreamNameMapper>();
        services.AddSingleton<IEventStoreSerializer, EventStoreSerializer>();
        
        services.AddScoped(typeof(IEventStoreRepository<>), typeof(EventStoreRepository<>));
        
        return services;
    }
}