using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Adapter
{
    public sealed class AsyncFuncAdapter<TContext> : IAsyncAction<TContext>
    {
        private readonly Func<TContext, CancellationToken, Task<Result>> func;

        private AsyncFuncAdapter(Func<TContext, CancellationToken, Task<Result>> func)
        {
            this.func = func;
        }

        public async Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            return await func.Invoke(context, cancellationToken).ConfigureAwait(false);
        }

        public static AsyncFuncAdapter<TContext> Create(Func<TContext, CancellationToken, Task<Result>> func)
        {
            return new AsyncFuncAdapter<TContext>(func);
        }

        public static AsyncFuncAdapter<TContext> Create(Func<TContext, Task<Result>> func)
        {
            return new AsyncFuncAdapter<TContext>((context, _) => func.Invoke(context));
        }
    }
}