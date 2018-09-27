using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Adapters.Action.AsyncFunc
{
    public class AsyncBlockingAction<TContext> : IAsyncFunc<TContext>
    {
        private readonly IAction<TContext> action;

        public AsyncBlockingAction(IAction<TContext> action)
        {
            this.action = action;
        }

        public Task InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            action.Invoke(context);
            return Task.CompletedTask;
        }
    }
}