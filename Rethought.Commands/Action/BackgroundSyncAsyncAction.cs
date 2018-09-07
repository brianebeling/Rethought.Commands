using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Action
{
    public class BackgroundSyncAsyncAction<TContext> : IAsyncAction<TContext>
    {
        private readonly IAction<TContext> action;

        public BackgroundSyncAsyncAction(IAction<TContext> action)
        {
            this.action = action;
        }

        public async Task InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            await Task.Run(() => action.Invoke(context), cancellationToken);
        }
    }
}