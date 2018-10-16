using System;
using System.Threading.Tasks;

namespace Rethought.Commands.Conditions
{
    public sealed class AsyncFuncCondition<TContext> : IAsyncCondition<TContext>
    {
        private readonly Func<TContext, Task<bool>> func;

        private AsyncFuncCondition(Func<TContext, Task<bool>> func)
        {
            this.func = func;
        }

        public Task<bool> SatisfiedAsync(TContext context)
        {
            return func.Invoke(context);
        }

        public static AsyncFuncCondition<TContext> Create(Func<TContext, Task<bool>> func)
        {
            return new AsyncFuncCondition<TContext>(func);
        }
    }
}