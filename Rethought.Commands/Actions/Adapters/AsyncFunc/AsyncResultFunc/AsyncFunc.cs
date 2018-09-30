using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Adapters.AsyncFunc.AsyncResultFunc
{
    public sealed class AsyncFunc<TContext> : IAsyncResultFunc<TContext>
    {
        private readonly IAsyncFunc<TContext> asyncFunc;

        private AsyncFunc(IAsyncFunc<TContext> asyncFunc)
        {
            this.asyncFunc = asyncFunc;
        }

        public async Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            await asyncFunc.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
            return Result.Completed;
        }

        public static AsyncFunc<TContext> Create(IAsyncFunc<TContext> asyncFunc)
            => new AsyncFunc<TContext>(asyncFunc);
    }
}