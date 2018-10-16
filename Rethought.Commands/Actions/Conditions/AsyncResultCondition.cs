using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Conditions;

namespace Rethought.Commands.Actions.Conditions
{
    public class AsyncResultCondition<TContext> : IAsyncResultFunc<TContext>
    {
        private readonly IAsyncCondition<TContext> asyncPrecondition;
        private readonly IAsyncResultFunc<TContext> success;

        public AsyncResultCondition(
            IAsyncCondition<TContext> asyncPrecondition,
            IAsyncResultFunc<TContext> success)
        {
            this.asyncPrecondition = asyncPrecondition;
            this.success = success;
        }

        public async Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            if (await asyncPrecondition.SatisfiedAsync(context).ConfigureAwait(false))
            {
                return await success.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
            }

            return Result.Aborted;
        }
    }
}