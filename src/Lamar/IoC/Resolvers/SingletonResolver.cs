using System;
using System.Diagnostics;
using JasperFx.Core;
using JasperFx.Core.Reflection;

namespace Lamar.IoC.Resolvers;

public abstract class SingletonResolver<T> : IResolver
{
    private readonly object _locker = new();
    private readonly Scope _topLevelScope;

    private T _service;

    public SingletonResolver(Scope topLevelScope)
    {
        _topLevelScope = topLevelScope;
    }

    public Type ServiceType => typeof(T);

    public object Resolve(Scope scope)
    {
        if (_service != null)
        {
            scope.WriteLine($"SingletonResolver._service - found: {Name}");
            return _service;
        }

        scope.WriteLine($"SingletonResolver._service - is null: {Name}");

        if (_topLevelScope.Services.TryFind(Hash, out var service))
        {
            scope.WriteLine($"SingletonResolver._topLevelScope.Services.TryFind() - found in top level: {Name}");
            _service = (T)service;
            return _service;
        }

        scope.WriteLine($"SingletonResolver lock(_locker) - before: {Name}");

        lock (_locker)
        {
            scope.WriteLine($"SingletonResolver lock(_locker) - start: {Name}");

            if (_service == null)
            {
                if (_topLevelScope.Services.TryFind(Hash, out var o))
                {
                    scope.WriteLine($"SingletonResolver._topLevelScope.Services.TryFind() - found in top level: {Name}");
                    _service = (T)o;
                }
                else
                {
                    scope.WriteLine($"SingletonResolver - creating new instance: {Name}");

                    _service = Build(_topLevelScope);
                    _topLevelScope.TryAddDisposable(_service);

                    _topLevelScope.Services = _topLevelScope.Services.AddOrUpdate(Hash, _service);
                }
            }

            scope.WriteLine($"SingletonResolver lock(_locker) - end: {Name}");
        }

        return _service;
    }

    public string Name { get; set; }
    public int Hash { get; set; }

    public abstract T Build(Scope scope);
}