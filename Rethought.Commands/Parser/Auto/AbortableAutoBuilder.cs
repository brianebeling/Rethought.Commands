using System;
using System.Collections.Generic;
using Optional;

namespace Rethought.Commands.Parser.Auto
{
    public class AbortableAutoBuilder
    {
        private readonly Dictionary<Type, IAbortableTypeParser<string, object>> typeParsers;

        public AbortableAutoBuilder(Dictionary<Type, IAbortableTypeParser<string, object>> typeParsers)
        {
            this.typeParsers = typeParsers;
        }

        public static AbortableAutoBuilder Create(Dictionary<Type, IAbortableTypeParser<string, object>> typeParsers)
        {
            return new AbortableAutoBuilder(typeParsers);
        }

        public static AbortableAutoBuilder Create()
        {
            return new AbortableAutoBuilder(new Dictionary<Type, IAbortableTypeParser<string, object>>());
        }

        public AbortableAutoBuilder WithTypeParser<TOutput>(IAbortableTypeParser<string, TOutput> typeParser)
        {
            typeParsers.Add(typeof(TOutput), new AbortableObjectTypeParserWrapper<string, TOutput>(typeParser));

            return this;
        }

        public AbortableAutoBuilder WithTypeParser<TOutput>(System.Func<string, (bool Completed, Option<TOutput> Output)> func)
        {
            typeParsers.Add(
                typeof(TOutput),
                new AbortableObjectTypeParserWrapper<string, TOutput>(AbortableFunc<string, TOutput>.Create(func)));

            return this;
        }

        public AbortableAutoTypeParser<TInput, TOutput> Build<TInput, TOutput>(System.Func<TInput, string> func)
        {
            return new AbortableAutoTypeParser<TInput, TOutput>(typeParsers, func);
        }
    }
}