using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Adapter
{
    public sealed class FuncAdapter<TContext> : IAsyncAction<TContext>
    {
        private readonly Func<TContext, CancellationToken, Task<Result>> func;

        private FuncAdapter(Func<TContext, CancellationToken, Task<Result>> func)
        {
            this.func = func;
        }

        public async Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            return await func.Invoke(context, cancellationToken).ConfigureAwait(false);
        }

        public static FuncAdapter<TContext> Create(Func<TContext, CancellationToken, Task<Result>> func)
        {
            return new FuncAdapter<TContext>(func);
        }

        public static FuncAdapter<TContext> Create(Func<TContext, Task<Result>> func)
        {
            return new FuncAdapter<TContext>((context, _) => func.Invoke(context));
        }

        public static FuncAdapter<TContext> Create(Func<TContext, CancellationToken, Task> func)
        {
            async Task<Result> Func(TContext context, CancellationToken token)
            {
                await func.Invoke(context, token);
                return Result.Completed;
            }

            return new FuncAdapter<TContext>(Func);
        }

        public static FuncAdapter<TContext> Create(Func<TContext, Task> func)
        {
            async Task<Result> Func(TContext context, CancellationToken _)
            {
                await func.Invoke(context);
                return Result.Completed;
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