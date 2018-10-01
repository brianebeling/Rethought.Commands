using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Adapters.System.Func
{
    public static class Extensions
    {
        public static IAsyncFunc<TContext> ToAsyncFunc<TContext>(this Func<TContext, CancellationToken, Task> func)
            => AsyncFunc.Func<TContext>.Create(func);

        public static IAsyncFunc<TContext> ToAsyncFunc<TContext>(this Func<TContext, Task> func)
            => AsyncFunc.Func<TContext>.Create(func);

        public static IResultFunc<TContext> ToResultFunc<TContext>(this Func<TContext, Result> func)
            => ResultFunc.Func<TContext>.Create(func);

        public static IAsyncResultFunc<TContext> ToAsyncResultFunc<TContext>(
            this Func<TContext, CancellationToken, Task<Result>> func)
            => AsyncResultFunc.Func<TContext>.Create(func);

        public static IAsyncResultFunc<TContext> ToAsyncResultFunc<TContext>(this Func<TContext, Task<Result>> func)
            => AsyncResultFunc.Func<TContext>.Create(func);
    }
}