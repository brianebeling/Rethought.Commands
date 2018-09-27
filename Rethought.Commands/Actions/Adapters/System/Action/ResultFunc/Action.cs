namespace Rethought.Commands.Actions.Adapters.System.Action.ResultFunc
{
    public sealed class Action<TContext> : IResultFunc<TContext>
    {
        private readonly global::System.Action<TContext> action;

        private Action(global::System.Action<TContext> action)
        {
            this.action = action;
        }

        public Result Invoke(TContext context)
        {
            action.Invoke(context);
            return Result.Completed;
        }

        public static Action<TContext> Create(global::System.Action<TContext> action)
        {
            return new Action<TContext>(action);
        }
    }
}