using Microsoft.EntityFrameworkCore;

namespace DeadLock.Services;

public class DeadLockDbContext : DbContext
{
    public DeadLockDbContext(DbContextOptions options) : base(options)
    {
    }

    public DeadLockDbContext() : this(new DbContextOptions<DeadLockDbContext>())
    {
    }
}
