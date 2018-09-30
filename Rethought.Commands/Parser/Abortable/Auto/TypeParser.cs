using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Optional;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Parser.Abortable.Auto
{
    public class TypeParser<TInput, TOutput> : ITypeParser<TInput, TOutput>
    {
        private const char GroupEnclosingChar = '"';

        private readonly Dictionary<Type, IAbortableTypeParser<string, object>> dictionary;
        private readonly System.Func<TInput, string> func;

        public TypeParser(
            Dictionary<Type, IAbortableTypeParser<string, object>> dictionary,
            System.Func<TInput, string> func)
        {
            this.dictionary = dictionary;
            this.func = func;
        }

        public Option<TOutput> Parse(TInput input)
        {
            var message = func.Invoke(input);

            var type = typeof(TOutput);

            var commandParameter = SplitInput(message);

            foreach (var constructor in type.GetConstructors())
            {
                var parsedParameters = new List<object>();
                var infos = constructor.GetParameters();

                for (var index = 0; index < infos.Length; index++)
                {
                    var constructorParameter = infos[index];

                    // TODO handle out of range exception..
                    var inputParameter = commandParameter[index];

                    // TODO handle out of range exception..
                    var typeParser = dictionary[constructorParameter.ParameterType];
                    if (typeParser.TryParse(inputParameter, out var output))
                    {
                        if (output.TryGetValue(out var value))
                        {
                            parsedParameters.Add(value);
                        }
                    }
                    else
                    {
                        return default;
                    }
                }

                if (parsedParameters.Any())
                {
                    var instance = constructor.Invoke(parsedParameters.ToArray());

                    return Option.Some((TOutput) instance);
                }
            }

            return default;
        }

        private IReadOnlyList<string> SplitInput(string input)
        {
            return input.Split(GroupEnclosingChar)
                .Select(
                    (element, index) =>
                    {
                        if (index % 2 == 0)
                        {
                            return element.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                        }

                        return new[] {element};
                    })
                .SelectMany(element => element)
                .ToImmutableList();
        }
    }
}