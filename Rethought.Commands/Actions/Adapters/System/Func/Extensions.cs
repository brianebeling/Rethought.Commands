using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Adapters.System.Func
{
    public static class Extensions
    {
        public static IAsyncFunc<TContext> ToAsyncFunc<TContext>(this Func<TContext, CancellationToken, Task> func)
        {
            return AsyncFunc.Func<TContext>.Create(func);
        }

        public static IAsyncFunc<TContext> ToAsyncFunc<TContext>(this Func<TContext, Task> func)
        {
            return AsyncFunc.Func<TContext>.Create(func);
        }

        public static IResultFunc<TContext> ToResultFunc<TContext>(this Func<TContext, Result> func)
        {
            return ResultFunc.Func<TContext>.Create(func);
        }

        public static IAsyncResultFunc<TContext> ToAsyncResultFunc<TContext>(
            this Func<TContext, CancellationToken, Task<Result>> func)
        {
            return AsyncResultFunc.Func<TContext>.Create(func);
        }

        public static IAsyncResultFunc<TContext> ToAsyncResultFunc<TContext>(this Func<TContext, Task<Result>> func)
        {
            return AsyncResultFunc.Func<TContext>.Create(func);
        }
    }
}