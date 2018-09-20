namespace Rethought.Commands.Actions
{
    public interface IAction<in TContext>
    {
        ActionResult Invoke(TContext context);
    }
}