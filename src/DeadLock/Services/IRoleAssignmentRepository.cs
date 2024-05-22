using Microsoft.EntityFrameworkCore;

namespace DeadLock.Services;

public interface IRoleAssignmentRepository
{
}

public class RoleAssignmentRepository : IRoleAssignmentRepository
{
    public RoleAssignmentRepository(
        IDbContextFactory<DeadLockDbContext> dbContextFactory)
    {
    }
}