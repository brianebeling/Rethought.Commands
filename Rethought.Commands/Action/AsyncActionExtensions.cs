namespace Rethought.Commands.Action
{
    public static class AsyncActionExtensions
    {
        public static IAsyncAction<TContext> ToBlockingAsyncAction<TContext>(this IAction<TContext> action)
        {
            return new BlockingSyncAsyncAction<TContext>(action);
        }

        public static IAsyncAction<TContext> ToBackgroundAsyncAction<TContext>(this IAction<TContext> action)
        {
            return new BackgroundSyncAsyncAction<TContext>(action);
        }
    }
}