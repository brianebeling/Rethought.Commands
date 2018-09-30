using Optional;

namespace Rethought.Commands.Parser
{
    public class ObjectTypeParserWrapper<TInput, TOutput> : ITypeParser<TInput, object>
    {
        private readonly ITypeParser<TInput, TOutput> typeParser;

        public ObjectTypeParserWrapper(ITypeParser<TInput, TOutput> typeParser)
        {
            this.typeParser = typeParser;
        }

        public Option<object> Parse(TInput input)
        {
            return typeParser.Parse(input).Map(innerOption => (object) innerOption);
        }
    }
}