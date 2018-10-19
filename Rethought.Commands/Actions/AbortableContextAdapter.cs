using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Parser;

namespace Rethought.Commands.Actions
{
    public class AbortableContextAdapter<TInput, TOutput> : IAsyncResultFunc<TInput>
    {
        private readonly IAsyncResultFunc<TOutput> command;
        private readonly IAbortableTypeParser<TInput, TOutput> parser;

        public AbortableContextAdapter(
            IAbortableTypeParser<TInput, TOutput> parser,
            IAsyncResultFunc<TOutput> command)
        {
            this.parser = parser;
            this.command = command;
        }

        public async Task<Result> InvokeAsync(TInput context, CancellationToken cancellationToken)
        {
            if (parser.TryParse(context, out var option))
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