using Rethought.Optional;

namespace Rethought.Commands.Parser
{
    public class InputMapTypeParser<TInput, TOutput, TNewInput> : ITypeParser<TInput, TOutput>
    {
        private readonly System.Func<TInput, TNewInput> func;
        private readonly ITypeParser<TNewInput, TOutput> typeParser;

        public InputMapTypeParser(ITypeParser<TNewInput, TOutput> typeParser, System.Func<TInput, TNewInput> func)
        {
            this.typeParser = typeParser;
            this.func = func;
        }


        Option<TOutput> ITypeParser<TInput, TOutput>.Parse(TInput input)
        {
            return typeParser.Parse(func.Invoke(input));
        }
    }
}