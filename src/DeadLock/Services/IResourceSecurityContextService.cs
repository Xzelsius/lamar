using Microsoft.EntityFrameworkCore;

namespace DeadLock.Services;

public interface IResourceSecurityContextService
{
}

public class ResourceSecurityContextService : IResourceSecurityContextService
{
    public ResourceSecurityContextService(
        IDbContextFactory<DeadLockDbContext> dbContextFactory)
    {
    }
}