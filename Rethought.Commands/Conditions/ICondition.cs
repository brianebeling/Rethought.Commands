namespace Rethought.Commands.Conditions
{
    public interface ICondition<in TContext>
    {
        bool Satisfied(TContext context);
    }
}