using Microsoft.Extensions.Options;

namespace DeadLock.Services;

public interface IProAuthManagementService
{
}

public class ProAuthManagementService : IProAuthManagementService
{
    public ProAuthManagementService(
        IServiceClientFactory serviceClientFactory,
        IOptions<ProAuthClientOptions> clientOptions,
        ILogger<ProAuthManagementService> logger)
    {
    }
}

public class ProAuthClientOptions
{
    public const string OptionsKey = "ProAuthApiClient";
}

public interface IServiceEndpointSettings
{
}

public class ServiceEndpointSettingsBase : IServiceEndpointSettings
{
}

public interface ITokenHandler
{
}

public class ProAuthTokenHandler : ITokenHandler
{
    public ProAuthTokenHandler(
        ILogger<ProAuthTokenHandler> logger,
        IServiceEndpointSettings serviceEndpointSettings,
        IOptions<ProAuthClientOptions> clientOptions)
    {
    }
}

public interface IServiceClientFactory
{
}

public class ServiceClientFactory : IServiceClientFactory
{
    public ServiceClientFactory(
        IServiceEndpointSettings serviceEndpointSettings,
        ITokenHandler tokenHandler,
        ILogger<ServiceClientFactory> logger,
        IEnumerable<IServiceClientOptionProvider> serviceClientOptionProviders)
    {
    }
}

public interface IServiceClientOptionProvider
{
}