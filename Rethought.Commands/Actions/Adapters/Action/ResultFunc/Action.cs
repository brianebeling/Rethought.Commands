namespace Rethought.Commands.Actions.Adapters.Action.ResultFunc
{
    public class Action<TContext> : IResultFunc<TContext>
    {
        private readonly IAction<TContext> action;

        public Action(IAction<TContext> action)
        {
            this.action = action;
        }

        public Result Invoke(TContext context)
        {
            action.Invoke(context);

            return Result.Completed;
        }
    }
}