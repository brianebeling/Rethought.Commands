using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Adapter
{
    public sealed class FuncAdapter<TContext> : IAsyncAction<TContext>
    {
        private readonly Func<TContext, CancellationToken, Task<ActionResult>> func;

        private FuncAdapter(Func<TContext, CancellationToken, Task<ActionResult>> func)
        {
            this.func = func;
        }

        public async Task<ActionResult> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            return await func.Invoke(context, cancellationToken).ConfigureAwait(false);
        }

        public static FuncAdapter<TContext> Create(Func<TContext, CancellationToken, Task<ActionResult>> func)
        {
            return new FuncAdapter<TContext>(func);
        }

        public static FuncAdapter<TContext> Create(Func<TContext, Task<ActionResult>> func)
        {
            return new FuncAdapter<TContext>((context, _) => func.Invoke(context));
        }

        public static FuncAdapter<TContext> Create(Func<TContext, CancellationToken, Task> func)
        {
            async Task<ActionResult> Func(TContext context, CancellationToken token)
            {
                await func.Invoke(context, token);
                return ActionResult.Completed;
            }

            return new FuncAdapter<TContext>(Func);
        }

        public static FuncAdapter<TContext> Create(Func<TContext, Task> func)
        {
            async Task<ActionResult> Func(TContext context, CancellationToken _)
            {
                await func.Invoke(context);
                return ActionResult.Completed;
            }

            return new FuncAdapter<TContext>(Func);
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