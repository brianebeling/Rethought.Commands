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

            if (newContext.HasValue)
            {
                return await command.InvokeAsync(newContext.ValueOr(default(TOutgoingContext)), cancellationToken).ConfigureAwait(false);
            }

            // This is dirty, but the only way to get the exception value of Option without modifying the source code or using reflection
            bool exception = default;
            newContext.Match(x => { }, b => exception = b);

            return exception ? Result.Aborted : Result.None;
        }
    }
}