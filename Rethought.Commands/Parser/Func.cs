using Optional;

namespace Rethought.Commands.Parser
{
    public sealed class Func<TInput, TOutput> : ITypeParser<TInput, TOutput>
    {
        private readonly System.Func<TInput, Option<Option<TOutput>>> func;

        private Func(System.Func<TInput, Option<Option<TOutput>>> func)
        {
            this.func = func;
        }

        public Option<Option<TOutput>> Parse(TInput input)
        {
            return func.Invoke(input);
        }

        public static Func<TInput, TOutput> Create(System.Func<TInput, Option<Option<TOutput>>> func)
        {
            return new Func<TInput, TOutput>(func);
        }
    }
}