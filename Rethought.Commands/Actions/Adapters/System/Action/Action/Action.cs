namespace Rethought.Commands.Actions.Adapters.System.Action.Action
{
    public class Action<TContext> : IAction<TContext>
    {
        private readonly global::System.Action<TContext> action;

        private Action(global::System.Action<TContext> action)
        {
            this.action = action;
        }

        public void Invoke(TContext context)
        {
            action.Invoke(context);
        }

        public static Action<TContext> Create(global::System.Action<TContext> action)
        {
            return new Action<TContext>(action);
        }
    }
}