using Marten;

namespace DeadLock.Services;

public interface IUserContextInfoResolver
{
}

public class UserContextInfoResolver : IUserContextInfoResolver
{
    public UserContextInfoResolver(
        IDocumentStore store,
        ITenantContext tenantContext)
    {
    }
}