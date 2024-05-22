namespace DeadLock.Services;

public interface ITenantIdProvider
{
}

public class DefaultTenantIdProvider : ITenantIdProvider
{
    public DefaultTenantIdProvider(
        IHttpContextAccessor httpContextAccessor,
        IEnumerable<ITenantIdResolver> tenantIdResolvers,
        ILogger<DefaultTenantIdProvider> logger)
    {
    }
}
