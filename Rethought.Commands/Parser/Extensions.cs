using Optional;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Parser
{
    public static class Extensions
    {
        public static IAbortableTypeParser<TNewInput, TOutput> Map<TInput, TOutput, TNewInput>(
            this IAbortableTypeParser<TInput, TOutput> abortableTypeParser,
            System.Func<TNewInput, TInput> func)
        {
            return Func<TNewInput, TOutput>.Create(
                input =>
                {
                    if (abortableTypeParser.TryParse(func.Invoke(input), out var option))
                    {
                        return option.TryGetValue(out var value) ? (true, Option.Some(value)) : (true, default);
                    }

                    return (false, default);
                });
        }
    }
}