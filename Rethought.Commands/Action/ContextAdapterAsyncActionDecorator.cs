using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Parser;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Action
{
    public class ContextAdapterAsyncActionDecorator<TIncomingContext, TOutgoingContext> : IAsyncAction<TIncomingContext>
    {
        private readonly ITypeParser<TIncomingContext, TOutgoingContext> parser;
        private readonly IAsyncAction<TOutgoingContext> command;

        public ContextAdapterAsyncActionDecorator(
            ITypeParser<TIncomingContext, TOutgoingContext> parser,
            IAsyncAction<TOutgoingContext> command)
        {
            this.parser = parser;
            this.command = command;
        }

        public async Task InvokeAsync(TIncomingContext context, CancellationToken cancellationToken)
        {
            var newContext = parser.Parse(context);

            if (newContext.TryGetValue(out var value))
            {
                await command.InvokeAsync(value, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}