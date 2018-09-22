using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions
{
    public class AsyncBlocking<TContext> : IAsyncAction<TContext>
    {
        private readonly IAction<TContext> action;

        public AsyncBlocking(IAction<TContext> action)
        {
            this.action = action;
        }

        public Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(action.Invoke(context));
        }
    }
}