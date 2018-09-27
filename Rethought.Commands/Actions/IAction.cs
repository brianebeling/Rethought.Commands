namespace Rethought.Commands.Actions
{
    public interface IAction<in TContext>
    {
        void Invoke(TContext context);
    }
}