using Rethought.Optional;

namespace Rethought.Commands.Parser
{
    public sealed class AbortableFunc<TInput, TOutput> : IAbortableTypeParser<TInput, TOutput>
    {
        private readonly System.Func<TInput, (bool Completed, Option<TOutput> Output)> func;

        private AbortableFunc(System.Func<TInput, (bool Completed, Option<TOutput> Output)> func)
        {
            this.func = func;
        }

        public bool TryParse(TInput input, out Option<TOutput> option)
        {
            var (completed, output) = func.Invoke(input);

            option = output;
            return completed;
        }


        public static AbortableFunc<TInput, TOutput> Create(System.Func<TInput, (bool Completed, Option<TOutput> Output)> func)
        {
            return new AbortableFunc<TInput, TOutput>(func);
        }
    }
}