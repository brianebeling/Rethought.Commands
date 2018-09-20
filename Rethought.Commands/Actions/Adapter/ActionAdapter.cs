using System;

namespace Rethought.Commands.Actions.Adapter
{
    public sealed class ActionAdapter<TContext> : IAction<TContext>
    {
        private readonly Action<TContext> action;

        private ActionAdapter(Action<TContext> action)
        {
            this.action = action;
        }

        public ActionResult Invoke(TContext context)
        {
            action.Invoke(context);
            return ActionResult.Completed;
        }

        public static IAction<TContext> Create(Action<TContext> action)
        {
            return new ActionAdapter<TContext>(action);
        }
    }
}