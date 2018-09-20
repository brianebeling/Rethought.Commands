using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Conditions;

namespace Rethought.Commands.Actions.Conditions
{
    public class AsyncCondition<TContext> : IAsyncAction<TContext>
    {
        private readonly IAsyncCondition<TContext> asyncPrecondition;
        private readonly IAsyncAction<TContext> success;

        public AsyncCondition(
            IAsyncCondition<TContext> asyncPrecondition,
            IAsyncAction<TContext> success)
        {
            this.asyncPrecondition = asyncPrecondition;
            this.success = success;
        }

        public async Task<ActionResult> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            if (await asyncPrecondition.SatisfiedAsync(context))
            {
                return await success.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
            }

            return ActionResult.Failed;
        }
    }
}