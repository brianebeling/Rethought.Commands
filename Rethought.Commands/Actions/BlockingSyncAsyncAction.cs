using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions
{
    public class BlockingSyncAsyncAction<TContext> : IAsyncAction<TContext>
    {
        private readonly IAction<TContext> action;

        public BlockingSyncAsyncAction(IAction<TContext> action)
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