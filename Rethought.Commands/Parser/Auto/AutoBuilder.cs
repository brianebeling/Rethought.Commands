using System;
using System.Collections.Generic;
using Rethought.Optional;

namespace Rethought.Commands.Parser.Auto
{
    public class AutoBuilder
    {
        private readonly Dictionary<Type, ITypeParser<string, object>> typeParsers;

        public AutoBuilder(Dictionary<Type, ITypeParser<string, object>> typeParsers)
        {
            this.typeParsers = typeParsers;
        }

        public static AutoBuilder Create(Dictionary<Type, ITypeParser<string, object>> typeParsers)
        {
            return new AutoBuilder(typeParsers);
        }

        public static AutoBuilder Create()
        {
            return new AutoBuilder(new Dictionary<Type, ITypeParser<string, object>>());
        }

        public AutoBuilder WithTypeParser<TOutput>(ITypeParser<string, TOutput> typeParser)
        {
            typeParsers.Add(typeof(TOutput), new ObjectTypeParserWrapper<string, TOutput>(typeParser));

            return this;
        }

        public AutoBuilder WithTypeParser<TOutput>(System.Func<string, Option<TOutput>> func)
        {
            typeParsers.Add(
                typeof(TOutput),
                new ObjectTypeParserWrapper<string, TOutput>(Func<string, TOutput>.Create(func)));

            return this;
        }

        public AutoTypeParser<TInput, TOutput> Build<TInput, TOutput>(System.Func<TInput, string> func)
        {
            return new AutoTypeParser<TInput, TOutput>(typeParsers, func);
        }
    }
}