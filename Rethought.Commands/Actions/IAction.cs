namespace Rethought.Commands.Actions
{
    public interface IAction<in TContext>
    {
        bool Invoke(TContext context);
    }
}