using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace Rethought.Commands.Parser
{
    public abstract class AsyncTypeParser<TInput, TOutput> : IAsyncTypeParser<TInput, TOutput>
    {
        public abstract Task<Option<Option<TOutput>>> ParseAsync(TInput input, CancellationToken cancellationToken);

        protected Task<Option<Option<TOutput>>> Aborted()
        {
            return Task.FromResult(default(Option<Option<TOutput>>));
        }
    }
}