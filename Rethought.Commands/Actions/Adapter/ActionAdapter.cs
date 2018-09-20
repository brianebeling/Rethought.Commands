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

        public bool Invoke(TContext context)
        {
            action.Invoke(context);
            return true;
        }

        public static IAction<TContext> Create(Action<TContext> action)
        {
            return new ActionAdapter<TContext>(action);
        }
    }
}