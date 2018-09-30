using System;

namespace Rethought.Commands.Actions.Adapters.System.Action
{
    public static class Extensions
    {
        public static IAction<TContext> ToAction<TContext>(this Action<TContext> action)
            => Action.Action<TContext>.Create(action);

        public static IResultFunc<TContext> ToResultFunc<TContext>(this Action<TContext> action)
            => ResultFunc.Action<TContext>.Create(action);
    }
}