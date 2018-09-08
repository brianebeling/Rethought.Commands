using System;
using System.Threading.Tasks;

namespace Rethought.Commands.Conditions
{
    public class AsyncFuncCondition<TContext> : IAsyncCondition<TContext>
    {
        private readonly Func<TContext, Task<ConditionResult>> func;

        private AsyncFuncCondition(Func<TContext, Task<ConditionResult>> func)
        {
            this.func = func;
        }

        public Task<ConditionResult> SatisfiedAsync(TContext context)
        {
            return func.Invoke(context);
        }

        public static AsyncFuncCondition<TContext> Create(Func<TContext, Task<ConditionResult>> func)
        {
            return new AsyncFuncCondition<TContext>(func);
        }
    }
}