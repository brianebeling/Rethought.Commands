using Rethought.Commands.Actions.Adapters.Action.AsyncFunc;
using Rethought.Commands.Actions.Adapters.Action.ResultFunc;

namespace Rethought.Commands.Actions.Adapters.Action
{
    public static class Extensions
    {
        public static IAsyncFunc<TContext> ToAsyncBlockingFunc<TContext>(this IAction<TContext> action)
            => new AsyncBlockingAction<TContext>(action);

        public static IAsyncFunc<TContext> ToAsyncBackgroundFunc<TContext>(this IAction<TContext> action)
            => new AsyncBackgroundAction<TContext>(action);

        public static IResultFunc<TContext> ToResultFunc<TContext>(this IAction<TContext> action)
            => new Action<TContext>(action);
    }
}