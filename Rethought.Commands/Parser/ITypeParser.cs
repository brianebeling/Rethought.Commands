using Optional;

namespace Rethought.Commands.Parser
{
    public interface ITypeParser<in TInput, TOutput>
    {
        Option<TOutput> Parse(TInput input);
    }
}