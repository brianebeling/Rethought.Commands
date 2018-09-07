using System;

namespace Rethought.Commands.Actions.Adapter
{
    public class ActionAdapter<TContext> : IAction<TContext>
    {
        private readonly Action<TContext> action;

        private ActionAdapter(Action<TContext> action)
        {
            this.action = action;
        }

        public void Invoke(TContext context)
        {
            action.Invoke(context);
        }

        public static IAction<TContext> Create(Action<TContext> action)
        {
            return new ActionAdapter<TContext>(action);
        }
    }
}