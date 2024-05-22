﻿using System;
using System.Diagnostics;
using JasperFx.Core.Reflection;

namespace Lamar.IoC.Resolvers;

public class SingletonLambdaResolver<TContainer, T> : SingletonResolver<T>
{
    private readonly Func<TContainer, T> _builder;

    public SingletonLambdaResolver(Func<TContainer, T> builder, Scope topLevelScope) : base(topLevelScope)
    {
        _builder = builder;
    }

    public override T Build(Scope scope)
    {
        scope.WriteLine("SingletonLambdaResolver: " + Name);
        return _builder(scope.As<TContainer>());
    }
}