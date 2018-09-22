using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace Rethought.Commands.Parser
{
    public interface IAsyncTypeParser<in TInput, TOutput>
    {
        Task<Option<TOutput, bool>> ParseAsync(TInput input, CancellationToken cancellationToken);
    }
}