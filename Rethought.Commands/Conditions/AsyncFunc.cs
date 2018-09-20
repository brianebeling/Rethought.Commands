using System;
using System.Threading.Tasks;

namespace Rethought.Commands.Conditions
{
    public sealed class AsyncFunc<TContext> : IAsyncCondition<TContext>
    {
        private readonly Func<TContext, Task<bool>> func;

        private AsyncFunc(Func<TContext, Task<bool>> func)
        {
            this.func = func;
        }

        public Task<bool> SatisfiedAsync(TContext context)
        {
            return func.Invoke(context);
        }

        public static AsyncFunc<TContext> Create(Func<TContext, Task<bool>> func)
        {
            return new AsyncFunc<TContext>(func);
        }
    }
}