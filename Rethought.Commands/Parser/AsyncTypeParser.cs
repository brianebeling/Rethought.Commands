using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace Rethought.Commands.Parser
{
    public abstract class AsyncTypeParser<TInput, TOutput> : IAsyncTypeParser<TInput, TOutput>
    {
        public abstract Task<Option<Option<TOutput>>> ParseAsync(TInput input, CancellationToken cancellationToken);

        protected Option<Option<TOutput>> Aborted() => default;
    }
}