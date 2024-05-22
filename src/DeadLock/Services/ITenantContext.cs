namespace DeadLock.Services;

public interface ITenantContext
{
}

public class WebTenantContext : ITenantContext
{
    public WebTenantContext(
        ITenantIdProvider tenantIdProvder,
        IProAuthManagementService proAuthManagementService,
        ILogger<WebTenantContext> logger)
    {
    }
}