namespace DeadLock.Services;

public interface IUserContext
{
}

public class WebUserContext : IUserContext
{
    public WebUserContext(
        IHttpContextAccessor httpContextAccessor,
        IUserContextInfoResolver userContextInfoResolver,
        ILogger<WebUserContext> logger)
    {
    }
}
