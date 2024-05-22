namespace DeadLock.Services;

public interface IConditionFactory
{
}

public class ConditionFactory : IConditionFactory
{
    public ConditionFactory(
        IExpressionCacheFactory expressionCacheFactory)
    {
    }
}