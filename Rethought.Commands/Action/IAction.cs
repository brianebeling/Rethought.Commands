namespace Rethought.Commands.Action
{
    public interface IAction<in TContext>
    {
        void Invoke(TContext context);
    }
}