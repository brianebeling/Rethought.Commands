using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Adapter
{
    public sealed class FuncAdapter<TContext> : IAsyncAction<TContext>
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

        public static FuncAdapter<TContext> Create(Func<TContext, CancellationToken, Task> func)
        {
            return new FuncAdapter<TContext>(func);
        }

        public static FuncAdapter<TContext> Create(Func<TContext, Task> func)
        {
            return new FuncAdapter<TContext>((context, _) => func.Invoke(context));
        }

        //public static implicit operator Func<TContext, CancellationToken, Task>(FuncAdapter<TContext> asyncAction)
        //{
        //    return asyncAction.InvokeAsync;
        //}

        //public static implicit operator FuncAdapter<TContext>(Func<TContext, CancellationToken, Task> func)
        //{
        //    return new FuncAdapter<TContext>(func);
        //}
    }
}