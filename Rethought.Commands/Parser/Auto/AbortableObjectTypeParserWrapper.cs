using Rethought.Optional;

namespace Rethought.Commands.Parser.Auto
{
    public class AbortableObjectTypeParserWrapper<TInput, TOutput> : IAbortableTypeParser<TInput, object>
    {
        private readonly IAbortableTypeParser<TInput, TOutput> typeParser;

        public AbortableObjectTypeParserWrapper(IAbortableTypeParser<TInput, TOutput> typeParser)
        {
            this.typeParser = typeParser;
        }


        public bool TryParse(TInput input, out Option<object> option)
        {
            if (typeParser.TryParse(input, out var output))
            {
                option = output;
                return true;
            }

            option = default;
            return false;
        }
    }
}