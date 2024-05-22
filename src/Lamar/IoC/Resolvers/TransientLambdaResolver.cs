using System;
using System.Diagnostics;
using JasperFx.Core.Reflection;

namespace Lamar.IoC.Resolvers;

public class TransientLambdaResolver<TContainer, T> : TransientResolver<T>
{
    private readonly Func<TContainer, T> _builder;

    public TransientLambdaResolver(Func<TContainer, T> builder)
    {
        _builder = builder;
    }

    public override T Build(Scope scope)
    {
        scope.WriteLine("TransientLambdaResolver: " + Name);
        return _builder(scope.As<TContainer>());
    }
}