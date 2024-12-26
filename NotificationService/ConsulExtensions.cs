using Consul;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;

public static class ConsulExtensions
{
    public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var address = configuration.GetValue<string>("ConsulConfig:ConsulAddress");
            consulConfig.Address = new Uri(address);
        }));
        return services;
    }

    public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
    {
        var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("ConsulExtensions");
        var lifetime = app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Hosting.IApplicationLifetime>();

        if (!(app.Properties["server.Features"] is FeatureCollection features)) return app;
        var registration = new AgentServiceRegistration();
        lifetime.ApplicationStarted.Register(() =>
        {
            var addresses = features.Get<IServerAddressesFeature>();
            var address = addresses.Addresses.First();

            Console.WriteLine($"address={address}");

            var uri = new Uri(address);
            registration = new AgentServiceRegistration()
            {
                ID = $"MyService-{uri.Port}",
                // servie name  
                Name = "MyService",
                Address = $"{uri.Host}",
                Port = uri.Port
            };

            logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);
        });

        lifetime.ApplicationStopping.Register(() =>
        {
            logger.LogInformation("Unregistering from Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
        });

        return app;
    }
}
