using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Conditions;

namespace Rethought.Commands.Actions.Conditions
{
    public class Condition<TContext> : IAsyncAction<TContext>
    {
        private readonly ICondition<TContext> precondition;
        private readonly IAsyncAction<TContext> success;

        public Condition(ICondition<TContext> precondition, IAsyncAction<TContext> success)
        {
            this.precondition = precondition;
            this.success = success;
        }

        public async Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            if (precondition.Satisfied(context))
            {
                return await success.InvokeAsync(context, cancellationToken);
            }

            return Result.None;
        }
    }
}