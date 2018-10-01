using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Adapters.ResultFunc.AsyncResultFunc
{
    public class AsyncBackgroundResultFunc<TContext> : IAsyncResultFunc<TContext>
    {
        private readonly IResultFunc<TContext> resultFunc;

        private AsyncBackgroundResultFunc(IResultFunc<TContext> resultFunc)
        {
            this.resultFunc = resultFunc;
        }

        public Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
            => Task.Run(() => resultFunc.Invoke(context), cancellationToken);

        public static AsyncBackgroundResultFunc<TContext> Create(IResultFunc<TContext> func)
            => new AsyncBackgroundResultFunc<TContext>(func);
    }
}