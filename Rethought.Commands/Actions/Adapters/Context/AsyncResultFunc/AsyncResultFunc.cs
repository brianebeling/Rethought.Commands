using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Parser;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Actions.Adapters.Context.AsyncResultFunc
{
    public class AsyncResultFunc<TInput, TOutput> : IAsyncResultFunc<TInput>
    {
        private readonly IAsyncAbortableTypeParser<TInput, TOutput> asyncAbortableParser;
        private readonly IAsyncResultFunc<TOutput> asyncResultFunc;

        public AsyncResultFunc(
            IAsyncAbortableTypeParser<TInput, TOutput> asyncAbortableParser,
            IAsyncResultFunc<TOutput> asyncResultFunc)
        {
            this.asyncAbortableParser = asyncAbortableParser;
            this.asyncResultFunc = asyncResultFunc;
        }

        public async Task<Result> InvokeAsync(TInput context, CancellationToken cancellationToken)
        {
            var newContext = await asyncAbortableParser.ParseAsync(context, cancellationToken).ConfigureAwait(false);

            if (newContext.Completed)
            {
                if (newContext.Result.TryGetValue(out var value))
                {
                    return await asyncResultFunc.InvokeAsync(value, cancellationToken).ConfigureAwait(false);
                }

                return Result.None;
            }

            return Result.Aborted;
        }
    }
}