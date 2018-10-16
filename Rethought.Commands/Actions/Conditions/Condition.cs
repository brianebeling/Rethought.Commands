using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Conditions;

namespace Rethought.Commands.Actions.Conditions
{
    public class Condition<TContext> : IAsyncResultFunc<TContext>
    {
        private readonly ICondition<TContext> precondition;
        private readonly IAsyncResultFunc<TContext> success;

        public Condition(ICondition<TContext> precondition, IAsyncResultFunc<TContext> success)
        {
            this.precondition = precondition;
            this.success = success;
        }

        public async Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            if (precondition.Satisfied(context))
            {
                return await success.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
            }

            return Result.None;
        }
    }
}