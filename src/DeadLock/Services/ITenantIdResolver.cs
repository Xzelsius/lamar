namespace DeadLock.Services;

public interface ITenantIdResolver
{
}

public class TenantIdResolver1 : ITenantIdResolver
{
    public TenantIdResolver1(ILogger<TenantIdResolver1> logger)
    {
    }
}

public class TenantIdResolver2 : ITenantIdResolver
{
    public TenantIdResolver2(ILogger<TenantIdResolver2> logger)
    {
    }
}
