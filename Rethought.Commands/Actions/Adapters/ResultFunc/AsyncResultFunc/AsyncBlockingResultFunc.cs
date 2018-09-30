using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Adapters.ResultFunc.AsyncResultFunc
{
    public class AsyncBlockingResultFunc<TContext> : IAsyncResultFunc<TContext>
    {
        private readonly IResultFunc<TContext> resultFunc;

        private AsyncBlockingResultFunc(IResultFunc<TContext> resultFunc)
        {
            this.resultFunc = resultFunc;
        }

        public Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
            => Task.FromResult(resultFunc.Invoke(context));

        public static AsyncBlockingResultFunc<TContext> Create(IResultFunc<TContext> func)
            => new AsyncBlockingResultFunc<TContext>(func);
    }
}