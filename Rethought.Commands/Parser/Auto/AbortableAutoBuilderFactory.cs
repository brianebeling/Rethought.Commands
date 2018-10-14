using System;
using System.Collections.Generic;

namespace Rethought.Commands.Parser.Auto
{
    public class AbortableAutoBuilderFactory
    {
        private readonly Dictionary<Type, IAbortableTypeParser<string, object>> typeParsers;

        public AbortableAutoBuilderFactory(Dictionary<Type, IAbortableTypeParser<string, object>> typeParsers)
        {
            this.typeParsers = typeParsers;
        }

        public AbortableAutoBuilder Create()
        {
            return new AbortableAutoBuilder(typeParsers);
        }
    }
}