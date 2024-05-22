namespace DeadLock.Services;

public interface ISecurityFilterContextFactory
{
}

public class SecurityFilterContextFactory : ISecurityFilterContextFactory
{
    public SecurityFilterContextFactory(
        IRoleAssignmentRepository roleAssignMentRepository,
        IUserContext userContext,
        IPermissionService permissionService)
    {
    }
}