using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Parser;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Actions
{
    public class AsyncContextAdapter<TIncomingContext, TOutgoingContext> : IAsyncAction<TIncomingContext>
    {
        private readonly IAsyncTypeParser<TIncomingContext, TOutgoingContext> asyncParser;
        private readonly IAsyncAction<TOutgoingContext> command;

        public AsyncContextAdapter(
            IAsyncTypeParser<TIncomingContext, TOutgoingContext> asyncParser,
            IAsyncAction<TOutgoingContext> command)
        {
            this.asyncParser = asyncParser;
            this.command = command;
        }

        public async Task<Result> InvokeAsync(TIncomingContext context, CancellationToken cancellationToken)
        {
            var newContext = await asyncParser.ParseAsync(context, cancellationToken).ConfigureAwait(false);

            if (newContext.TryGetValue(out var option))
            {
                if (option.TryGetValue(out var value))
                {
                    return await command.InvokeAsync(value, cancellationToken).ConfigureAwait(false);
                }

                return Result.None;
            }

            return Result.Aborted;
        }
    }
}