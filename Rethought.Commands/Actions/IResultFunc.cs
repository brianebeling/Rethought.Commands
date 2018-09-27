namespace Rethought.Commands.Actions
{
    public interface IResultFunc<in TContext>
    {
        Result Invoke(TContext context);
    }
}