using System;
using System.Collections.Generic;

namespace Rethought.Commands.Parser.Auto
{
    public class AutoBuilderFactory
    {
        private readonly Dictionary<Type, ITypeParser<string, object>> typeParsers;

        public AutoBuilderFactory(Dictionary<Type, ITypeParser<string, object>> typeParsers)
        {
            this.typeParsers = typeParsers;
        }

        public AutoBuilder Create()
        {
            return new AutoBuilder(typeParsers);
        }
    }
}
