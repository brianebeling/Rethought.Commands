using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions
{
    public class AsyncBackground<TContext> : IAsyncAction<TContext>
    {
        private readonly IAction<TContext> action;

        public AsyncBackground(IAction<TContext> action)
        {
            this.action = action;
        }

        public async Task<bool> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            return await Task.Run(() => action.Invoke(context), cancellationToken);
        }
    }
}