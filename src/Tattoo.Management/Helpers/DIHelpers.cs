using System.Reflection.PortableExecutable;
using Tattoo.Management.Models.Configuration;

namespace Tattoo.Management.Helpers;

/// <summary>
/// Dependency Injection (DI) helper methods to ease the configuration of dependencies in the container.
/// </summary>
public static class DIHelpers
{
    /// <summary>
    /// Configures all the <see cref="HttpClient"/> instances in the <see cref="IHttpClientFactory"/>.
    /// </summary>
    public static IServiceCollection AddApiCallerClients(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var clientsConfig = configuration.GetRequiredSection("ClientConfigurations").Get<HttpClientConfiguration[]>();
        var apiBaseUrl = configuration["ApiBaseUrl"];
        var localBaseUrl = configuration["localBaseUrl"];

        if (clientsConfig is not null)
        {
            foreach (var clientConfig in clientsConfig)
            {
                services.AddHttpClient(clientConfig.Name, client =>
                {
                    client.BaseAddress = new Uri(new Uri(clientConfig.ClientTypeRemote ? apiBaseUrl : localBaseUrl), clientConfig.RelativePath);
                    client.Timeout = TimeSpan.FromSeconds(clientConfig.TimeOutSeconds);

                    if (clientConfig.DefaultHeaders is not null)
                    {
                        foreach (var header in clientConfig.DefaultHeaders)
                        {
                            if (!client.DefaultRequestHeaders.Contains(header.Key))
                            {
                                client.DefaultRequestHeaders.Add(header.Key, header.Value);
                            }
                        }
                    }
                });
            }
        }
        return services;
    }


}