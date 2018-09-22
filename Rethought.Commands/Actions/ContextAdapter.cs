using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Parser;

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

            if (newContext.HasValue)
            {
                return await command.InvokeAsync(newContext.ValueOr(default(TOutgoingContext)), cancellationToken);
            }

            // This is dirty, but the only way to get the exception value of Option without modifying the source code or using reflection
            bool exception = default;
            newContext.Match(x => { }, b => exception = b);

            return exception ? Result.Aborted : Result.None;
        }
    }
}