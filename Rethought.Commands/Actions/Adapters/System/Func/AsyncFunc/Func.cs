using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Adapters.System.Func.AsyncFunc
{
    public sealed class Func<TContext> : IAsyncFunc<TContext>
    {
        private readonly Func<TContext, CancellationToken, Task> func;

        private Func(Func<TContext, CancellationToken, Task> func)
        {
            this.func = func;
        }

        public async Task InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            await func.Invoke(context, cancellationToken).ConfigureAwait(false);
        }

        public static Func<TContext> Create(Func<TContext, CancellationToken, Task> func)
        {
            return new Func<TContext>(func);
        }

        public static Func<TContext> Create(Func<TContext, Task> func)
        {
            return new Func<TContext>((context, _) => func.Invoke(context));
        }
    }
}