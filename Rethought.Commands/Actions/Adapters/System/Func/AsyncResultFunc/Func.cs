using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Adapters.System.Func.AsyncResultFunc
{
    public sealed class Func<TContext> : IAsyncResultFunc<TContext>
    {
        private readonly Func<TContext, CancellationToken, Task<Result>> func;

        private Func(Func<TContext, CancellationToken, Task<Result>> func)
        {
            this.func = func;
        }

        public async Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            return await func.Invoke(context, cancellationToken).ConfigureAwait(false);
        }

        public static Func<TContext> Create(Func<TContext, CancellationToken, Task<Result>> func)
        {
            return new Func<TContext>(func);
        }

        public static Func<TContext> Create(Func<TContext, Task<Result>> func)
        {
            return new Func<TContext>((context, _) => func.Invoke(context));
        }
    }
}