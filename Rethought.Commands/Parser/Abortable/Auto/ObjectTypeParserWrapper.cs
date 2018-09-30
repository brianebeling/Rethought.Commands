using Optional;

namespace Rethought.Commands.Parser.Abortable.Auto
{
    public class ObjectTypeParserWrapper<TInput, TOutput> : IAbortableTypeParser<TInput, object>
    {
        private readonly IAbortableTypeParser<TInput, TOutput> typeParser;

        public ObjectTypeParserWrapper(IAbortableTypeParser<TInput, TOutput> typeParser)
        {
            this.typeParser = typeParser;
        }


        public bool TryParse(TInput input, out Option<object> option)
        {
            if (typeParser.TryParse(input, out var output))
            {
                option = output.Map(value => (object) value);
                return true;
            }

            option = default;
            return false;
        }
    }
}