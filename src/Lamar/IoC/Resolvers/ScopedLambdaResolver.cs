using System;
using System.Diagnostics;
using JasperFx.Core.Reflection;

namespace Lamar.IoC.Resolvers;

public class ScopedLambdaResolver<TContainer, T> : ScopedResolver<T>
{
    private readonly Func<TContainer, T> _builder;

    public ScopedLambdaResolver(Func<TContainer, T> builder)
    {
        _builder = builder;
    }

    public override T Build(Scope scope)
    {

        scope.WriteLine("ScopedLambdaResolver: " + Name);
        return _builder(scope.As<TContainer>());
    }
}