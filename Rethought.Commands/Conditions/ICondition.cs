namespace Rethought.Commands.Conditions
{
    public interface ICondition<in TContext>
    {
        ConditionResult Satisfied(TContext context);
    }
}