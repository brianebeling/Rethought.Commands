namespace Rethought.Commands.Actions
{
    public static class AsyncActionExtensions
    {
        public static IAsyncAction<TContext> ToAsyncBlocking<TContext>(this IAction<TContext> action)
        {
            return new AsyncBlocking<TContext>(action);
        }

        public static IAsyncAction<TContext> ToAsyncBackground<TContext>(this IAction<TContext> action)
        {
            return new AsyncBackground<TContext>(action);
        }
    }
}