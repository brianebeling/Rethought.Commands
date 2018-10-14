using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace Rethought.Commands.Parser
{
    /// <summary>
    ///     Attempts to parse from <see cref="TInput" /> to <see cref="TOutput" />
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    public interface IAsyncAbortableTypeParser<in TInput, TOutput>
    {
        /// <summary>
        ///     Attempts to parse. The first Option is none when the parsing was aborted. The second Option is none when the
        ///     parsing was unsuccessful.
        /// </summary>
        /// <param name="input">The input <see cref="TInput" />.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<(bool Completed, Option<TOutput> Result)> ParseAsync(TInput input, CancellationToken cancellationToken);
    }
}