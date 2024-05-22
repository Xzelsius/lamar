using Microsoft.FeatureManagement;

namespace DeadLock.Services;

public interface IPermissionService
{
}

public class PermissionService : IPermissionService
{
    public PermissionService(
        IResourceSecurityContextService resourceSecurityContextService,
        IRoleAssignmentRepository roleAssignmentRepository,
        IConditionFactory conditionFactory,
        IFeatureManager featureManager)
    {
    }
}