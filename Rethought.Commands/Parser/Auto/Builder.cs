using System;
using System.Collections.Generic;
using Optional;

namespace Rethought.Commands.Parser.Auto
{
    public class Builder
    {
        private readonly Dictionary<Type, ITypeParser<string, object>> typeParsers;

        public Builder(Dictionary<Type, ITypeParser<string, object>> typeParsers)
        {
            this.typeParsers = typeParsers;
        }

        public static Builder Create(Dictionary<Type, ITypeParser<string, object>> typeParsers)
        {
            return new Builder(typeParsers);
        }

        public static Builder Create()
        {
            return new Builder(new Dictionary<Type, ITypeParser<string, object>>());
        }

        public Builder WithTypeParser<TOutput>(ITypeParser<string, TOutput> typeParser)
        {
            typeParsers.Add(typeof(TOutput), new ObjectTypeParserWrapper<string, TOutput>(typeParser));

            return this;
        }

        public Builder WithTypeParser<TOutput>(System.Func<string, Option<TOutput>> func)
        {
            typeParsers.Add(
                typeof(TOutput),
                new ObjectTypeParserWrapper<string, TOutput>(Parser.Func<string, TOutput>.Create(func)));

            return this;
        }

        public TypeParser<TInput, TOutput> Build<TInput, TOutput>(System.Func<TInput, string> func)
        {
            return new TypeParser<TInput, TOutput>(typeParsers, func);
        }
    }
}