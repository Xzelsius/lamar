using DeadLock.Services;
using Lamar;
using Lamar.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace DeadLock.Tests;

public sealed class DeadLockTest
{
    [Fact(Timeout = 30_000)]
    public async Task Does_not_deadlock_Endpoints()
    {
        await using var host = await Alba.AlbaHost.For<Program>();

        var requests = new string[]
        {
            "/r1",
            "/r2",
            "/r3",
            "/r4",
            "/r5",
            "/r6",
        };

        await Parallel.ForEachAsync(requests, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (request, ct) =>
        {
            await host.Scenario(_ => _.Get.Url(request));
        });
    }

    [Fact(Timeout = 30_000)]
    public async Task Does_not_deadlock_IServiceProvider()
    {
        await using var host = await Alba.AlbaHost.For<Program>();

        Debug.WriteLine("#################################################################################################################");

        var container = host.Services.GetRequiredService<IServiceProvider>();

        var r1Singletons = new List<Type>
        {
            typeof(WolverineOptions),
            typeof(IServiceProvider),
            typeof(IProAuthManagementService),
        };
        var r1Scoped = new List<Type>
        {
            typeof(IUserContext),
        };

        var r2Singletons = new List<Type>
        {
            typeof(WolverineOptions),
            typeof(IServiceProvider),
        };
        var r2Scoped = new List<Type>
        {
            typeof(ISecurityFilterContextFactory),
            typeof(IDbContextFactory<DeadLockDbContext>),
        };

        var r3Singletons = new List<Type>
        {
            typeof(WolverineOptions),
            typeof(IServiceProvider),
        };
        var r3Scoped = new List<Type>
        {
            typeof(ISecurityFilterContextFactory),
            typeof(IDbContextFactory<DeadLockDbContext>),
        };

        var r4Singletons = new List<Type>
        {
            typeof(WolverineOptions),
            typeof(IServiceProvider),
        };
        var r4Scoped = new List<Type>
        {
            typeof(IPermissionService),
            typeof(IUserContext),
        };

        var r5Singletons = new List<Type>
        {
            typeof(WolverineOptions),
            typeof(IServiceProvider),
        };
        var r5Scoped = new List<Type>
        {
            typeof(ISecurityFilterContextFactory),
            typeof(IDbContextFactory<DeadLockDbContext>),
        };

        var r6Singletons = new List<Type>
        {
            typeof(WolverineOptions),
            typeof(IServiceProvider),
        };
        var r6Scoped = new List<Type>
        {
            typeof(IDbContextFactory<DeadLockDbContext>),
            typeof(IUserContext),
        };

        (int Id, List<Type> Singletons, List<Type> Scoped)[] requests =
        [
            (1, r1Singletons, r1Scoped),
            (2, r2Singletons, r2Scoped),
            (3, r3Singletons, r3Scoped),
            (4, r4Singletons, r4Scoped),
            (5, r5Singletons, r5Scoped),
            (6, r6Singletons, r6Scoped),
        ];

        await Parallel.ForEachAsync(requests, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (request, ct) =>
        {
            //(container as Scope).WriteLine($"Start R{request.Id} (Singleton)");

            var singletonInstances = request.Singletons.Select(t => container.GetRequiredService(t)).ToList();

            //(container as Scope).WriteLine($"End R{request.Id} (Singleton)");

            var root = singletonInstances.OfType<IServiceProvider>().Single();
            using var scope = root.CreateScope();

            //(container as Scope).WriteLine($"Start R{request.Id} (Scoped)");

            _ = scope.ServiceProvider.GetRequiredService<ITenantIdProvider>();
            // normely there would be some interaction with ITenantIdProvider here

            var scopedInstances = request.Scoped.Select(t => scope.ServiceProvider.GetRequiredService(t)).ToList();

            //(container as Scope).WriteLine($"End R{request.Id} (Scoped)");

            await Task.Delay(1000);
        });

        Debug.WriteLine("#################################################################################################################");
    }
}
