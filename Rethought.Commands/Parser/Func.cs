using Optional;

namespace Rethought.Commands.Parser
{
    public sealed class Func<TInput, TOutput> : IAbortableTypeParser<TInput, TOutput>
    {
        private readonly System.Func<TInput, (bool Completed, Option<TOutput> Output)> func;

        private Func(System.Func<TInput, (bool Completed, Option<TOutput> Output)> func)
        {
            this.func = func;
        }

        public bool TryParse(TInput input, out Option<TOutput> option)
        {
            var (completed, output) = func.Invoke(input);

            option = output;
            return completed;
        }


        public static Func<TInput, TOutput> Create(System.Func<TInput, (bool Completed, Option<TOutput> Output)> func)
        {
            return new Func<TInput, TOutput>(func);
        }
    }
}