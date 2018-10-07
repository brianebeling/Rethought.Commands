using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Optional;
using Optional.Collections;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Parser.Auto
{
    public class TypeParser<TInput, TOutput> : ITypeParser<TInput, TOutput>
    {
        // TODO make configurable
        private const char GroupEnclosingChar = '"';

        private readonly Dictionary<Type, ITypeParser<string, object>> dictionary;
        private readonly System.Func<TInput, string> func;

        public TypeParser(
            Dictionary<Type, ITypeParser<string, object>> dictionary,
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

            if (!type.GetConstructors().FirstOrNone().TryGetValue(out var constructor)) return default;

            var parsedParameters = new List<object>();
            var infos = constructor.GetParameters();

            var skippedParameters = 0;
            for (var index = 0; index < infos.Length; index++)
            {
                var constructorParameter = infos[index];
                var constructorParameterIsOption = constructorParameter.ParameterType == typeof(Option<>);

                if (constructorParameter.ParameterType == typeof(TOutput))
                {
                    parsedParameters.Add(input);
                    skippedParameters++;
                    continue;
                }

                if (commandParameter.ElementAtOrNone(index - skippedParameters).TryGetValue(out var inputParameter))
                {
                    var typeParser = dictionary[constructorParameter.ParameterType];
                    var typeParserResultOption = typeParser.Parse(inputParameter);

                    if (typeParserResultOption.TryGetValue(out var typeParserResult))
                    {
                        parsedParameters.Add(typeParserResult);
                    }
                    else if (!constructorParameterIsOption)
                    {
                        return default;
                    }
                }
                else if (!constructorParameterIsOption)
                {
                    return default;
                }
            }

            var instance = constructor.Invoke(parsedParameters.ToArray());

            return Option.Some((TOutput) instance);
        }

        /// <summary>
        /// Splits the input into individual parameters. Everything inside a <see cref="GroupEnclosingChar"/> is considered as one parameter.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private static IReadOnlyList<string> SplitInput(string input)
        {
            return input.Split(GroupEnclosingChar)
                .Select(
                    (element, index) =>
                        index % 2 == 0
                            ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                            : new[] { element })
                .SelectMany(element => element)
                .ToImmutableList();
        }
    }
}