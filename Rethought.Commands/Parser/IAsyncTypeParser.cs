using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace Rethought.Commands.Parser
{
    public interface IAsyncTypeParser<in TInput, TOutput>
    {
        Task<Option<TOutput>> ParseAsync(TInput input, CancellationToken cancellationToken);
    }
}