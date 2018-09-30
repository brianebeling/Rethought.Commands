using System;
using System.Collections.Generic;
using Optional;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Parser.Abortable.Auto
{
    public class Builder
    {
        private readonly Dictionary<Type, IAbortableTypeParser<string, object>> typeParsers;

        public Builder(Dictionary<Type, IAbortableTypeParser<string, object>> typeParsers)
        {
            this.typeParsers = typeParsers;
        }

        public static Builder Create(Dictionary<Type, IAbortableTypeParser<string, object>> typeParsers)
        {
            return new Builder(typeParsers);
        }

        public static Builder Create()
        {
            return new Builder(new Dictionary<Type, IAbortableTypeParser<string, object>>());
        }

        public Builder WithTypeParser<TOutput>(IAbortableTypeParser<string, TOutput> typeParser)
        {
            typeParsers.Add(typeof(TOutput), new ObjectTypeParserWrapper<string, TOutput>(typeParser));

            return this;
        }

        public Builder WithTypeParser<TOutput>(System.Func<string, (bool Completed, Option<TOutput> Output)> func)
        {
            typeParsers.Add(
                typeof(TOutput),
                new ObjectTypeParserWrapper<string, TOutput>(Parser.Abortable.Func<string, TOutput>.Create(func)));

            return this;
        }

        public TypeParser<TInput, TOutput> Build<TInput, TOutput>(System.Func<TInput, string> func)
        {
            return new TypeParser<TInput, TOutput>(typeParsers, func);
        }
    }
}