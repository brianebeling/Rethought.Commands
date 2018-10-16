using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Adapters.Action.AsyncFunc
{
    public class AsyncBackgroundAction<TContext> : IAsyncFunc<TContext>
    {
        private readonly IAction<TContext> action;

        public AsyncBackgroundAction(IAction<TContext> action)
        {
            this.action = action;
        }

        public async Task InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            await Task.Run(() => action.Invoke(context), cancellationToken).ConfigureAwait(false);
        }
    }
}