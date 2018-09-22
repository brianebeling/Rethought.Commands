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

    public sealed class FuncAdapter<TContext> : IAction<TContext>
    {
        private readonly Func<TContext, Result> func;

        private FuncAdapter(Func<TContext, Result> func)
        {
            this.func = func;
        }

        public Result Invoke(TContext context)
        {
            return func.Invoke(context);
        }

        public static FuncAdapter<TContext> Create(Func<TContext, Result> func)
        {
            return new FuncAdapter<TContext>(func);
        }
    }
}