using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Conditions;

namespace Rethought.Commands.Actions.Conditions
{
    public class AsyncConditionalAsyncAction<TContext> : IAsyncAction<TContext>
    {
        private readonly IAsyncCondition<TContext> asyncPrecondition;
        private readonly IAsyncAction<TContext> success;

        public AsyncConditionalAsyncAction(
            IAsyncCondition<TContext> asyncPrecondition,
            IAsyncAction<TContext> success)
        {
            this.asyncPrecondition = asyncPrecondition;
            this.success = success;
        }

        public async Task InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            if ((await asyncPrecondition.SatisfiedAsync(context).ConfigureAwait(false)).Satisfied)
            {
                await success.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}