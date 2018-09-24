using Optional;

namespace Rethought.Commands.Parser
{
    public abstract class TypeParser<TInput, TOutput> : ITypeParser<TInput, TOutput>
    {
        public abstract Option<Option<TOutput>> Parse(TInput input);

        protected Option<Option<TOutput>> Aborted()
        {
            return default;
        }
    }
}