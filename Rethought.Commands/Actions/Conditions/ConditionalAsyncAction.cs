using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Conditions;

namespace Rethought.Commands.Actions.Conditions
{
    public class ConditionalAsyncAction<TContext> : IAsyncAction<TContext>
    {
        private readonly ICondition<TContext> precondition;
        private readonly IAsyncAction<TContext> success;

        public ConditionalAsyncAction(ICondition<TContext> precondition, IAsyncAction<TContext> success)
        {
            this.precondition = precondition;
            this.success = success;
        }

        public Task InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            if (precondition.Satisfied(context))
            {
                success.InvokeAsync(context, cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}