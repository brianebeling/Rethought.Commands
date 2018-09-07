using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Parser;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Action
{
    public class AsyncContextAdapterAsyncActionDecorator<TIncomingContext, TOutgoingContext> : IAsyncAction<TIncomingContext>
    {
        private readonly IAsyncTypeParser<TIncomingContext, TOutgoingContext> asyncParser;
        private readonly IAsyncAction<TOutgoingContext> command;

        public AsyncContextAdapterAsyncActionDecorator(
            IAsyncTypeParser<TIncomingContext, TOutgoingContext> asyncParser,
            IAsyncAction<TOutgoingContext> command)
        {
            this.asyncParser = asyncParser;
            this.command = command;
        }

        public async Task InvokeAsync(TIncomingContext context, CancellationToken cancellationToken)
        {
            var newContext = await asyncParser.ParseAsync(context, cancellationToken).ConfigureAwait(false);

            if (newContext.TryGetValue(out var value))
            {
                await command.InvokeAsync(value, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}