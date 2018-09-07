using System;
using System.Threading.Tasks;

namespace Rethought.Commands.Conditions
{
    public class AsyncFuncCondition<TContext> : IAsyncCondition<TContext>
    {
        private readonly Func<TContext, Task<ConditionResult>> func;

        public AsyncFuncCondition(Func<TContext, Task<ConditionResult>> func)
        {
            this.func = func;
        }

        public Task<ConditionResult> SatisfiedAsync(TContext context)
        {
            return func.Invoke(context);
        }
    }
}