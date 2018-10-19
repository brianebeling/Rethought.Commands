using Rethought.Optional;

namespace Rethought.Commands.Parser
{
    /// <summary>
    ///     Attempts to parse from <see cref="TInput" /> to <see cref="TOutput" />
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    public interface IAbortableTypeParser<in TInput, TOutput>
    {
        /// <summary>
        ///     Attempts to parse. Returns false when the parsing was aborted. The Option is none when the
        ///     parsing was unsuccessful.
        /// </summary>
        /// <param name="input">The input <see cref="TInput" />.</param>
        /// <param name="option"></param>
        bool TryParse(TInput input, out Option<TOutput> option);
    }
}