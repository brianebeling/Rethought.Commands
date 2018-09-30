using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Parser;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Actions
{
    public class ContextAdapter<TInput, TOutput> : IAsyncResultFunc<TInput>
    {
        private readonly IAsyncResultFunc<TOutput> command;
        private readonly IAbortableTypeParser<TInput, TOutput> parser;

        public ContextAdapter(
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
                    return await command.InvokeAsync(value, cancellationToken);
                }

                return Result.None;
            }

            return Result.Aborted;
        }
    }
}