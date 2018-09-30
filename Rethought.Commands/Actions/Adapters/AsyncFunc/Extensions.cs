using Rethought.Commands.Actions.Adapters.AsyncFunc.AsyncResultFunc;

namespace Rethought.Commands.Actions.Adapters.AsyncFunc
{
    public static class Extensions
    {
        public static IAsyncResultFunc<TContext> ToAsyncResultFunc<TContext>(this IAsyncFunc<TContext> func)
            => AsyncFunc<TContext>.Create(func);
    }
}