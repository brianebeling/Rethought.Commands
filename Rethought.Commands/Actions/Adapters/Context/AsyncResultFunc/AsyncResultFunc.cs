using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Parser;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Actions.Adapters.Context.AsyncResultFunc
{
    public class AsyncResultFunc<TInput, TOutput> : IAsyncResultFunc<TInput>
    {
        private readonly IAsyncTypeParser<TInput, TOutput> asyncParser;
        private readonly IAsyncResultFunc<TOutput> asyncResultFunc;

        public AsyncResultFunc(
            IAsyncTypeParser<TInput, TOutput> asyncParser,
            IAsyncResultFunc<TOutput> asyncResultFunc)
        {
            this.asyncParser = asyncParser;
            this.asyncResultFunc = asyncResultFunc;
        }

        public async Task<Result> InvokeAsync(TInput context, CancellationToken cancellationToken)
        {
            var newContext = await asyncParser.ParseAsync(context, cancellationToken).ConfigureAwait(false);

            if (newContext.TryGetValue(out var option))
            {
                if (option.TryGetValue(out var value))
                {
                    return await asyncResultFunc.InvokeAsync(value, cancellationToken).ConfigureAwait(false);
                }

                return Result.None;
            }

            return Result.Aborted;
        }
    }
}