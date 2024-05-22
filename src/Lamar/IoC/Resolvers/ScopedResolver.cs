using System;
using System.Diagnostics;
using JasperFx.Core;
using JasperFx.Core.Reflection;

namespace Lamar.IoC.Resolvers;

public abstract class ScopedResolver<T> : IResolver
{
    private readonly object _locker = new();
    public Type ServiceType => typeof(T);

    public object Resolve(Scope scope)
    {
        if (scope.Services.TryFind(Hash, out var service))
        {
            scope.WriteLine($"ScopedResolver.Services.TryFind() - found: {Name}");
            return service;
        }

        scope.WriteLine($"ScopedResolver.Services.TryFind() - not found: {Name}");
        scope.WriteLine($"ScopedResolver lock(_locker) - before: {Name}");

        lock (_locker)
        {
            scope.WriteLine($"ScopedResolver lock(_locker) - start: {Name}");

            if (scope.Services.TryFind(Hash, out service))
            {
                scope.WriteLine($"ScopedResolver.Services.TryFind() - found: {Name}");
                return service;
            }

            service = Build(scope);
            scope.Services = scope.Services.AddOrUpdate(Hash, service);

            scope.TryAddDisposable(service);

            scope.WriteLine($"ScopedResolver lock(_locker) - end: {Name}");

            return service;
        }
    }

    public string Name { get; set; }
    public int Hash { get; set; }

    public abstract T Build(Scope scope);
}