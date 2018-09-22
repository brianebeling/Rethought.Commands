namespace Rethought.Commands.Actions
{
    public interface IAction<in TContext>
    {
        Result Invoke(TContext context);
    }
}