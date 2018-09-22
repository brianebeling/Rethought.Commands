using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Parser;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Actions
{
    public class ContextAdapter<TIncomingContext, TOutgoingContext> : IAsyncAction<TIncomingContext>
    {
        private readonly IAsyncAction<TOutgoingContext> command;
        private readonly ITypeParser<TIncomingContext, TOutgoingContext> parser;

        public ContextAdapter(
            ITypeParser<TIncomingContext, TOutgoingContext> parser,
            IAsyncAction<TOutgoingContext> command)
        {
            this.parser = parser;
            this.command = command;
        }

        public async Task<Result> InvokeAsync(TIncomingContext context, CancellationToken cancellationToken)
        {
            var newContext = parser.Parse(context);

            if (newContext.TryGetValue(out var value))
            {
                return await command.InvokeAsync(value, cancellationToken).ConfigureAwait(false);
            }

            return Result.None;
        }
    }
}