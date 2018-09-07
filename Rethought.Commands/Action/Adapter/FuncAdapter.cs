using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Action.Adapter
{
    public class FuncAdapter<TContext> : IAsyncAction<TContext>
    {
        private readonly Func<TContext, CancellationToken, Task> func;

        private FuncAdapter(Func<TContext, CancellationToken, Task> func)
        {
            this.func = func;
        }

        public async Task InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            await func.Invoke(context, cancellationToken).ConfigureAwait(false);
        }

        public static IAsyncAction<TContext> Create(Func<TContext, CancellationToken, Task> func)
        {
            return new FuncAdapter<TContext>(func);
        }

        public static IAsyncAction<TContext> Create(Func<TContext, Task> func)
        {
            return new FuncAdapter<TContext>((context, _) => func.Invoke(context));
        }
    }
}