using System;
using System.Collections.Generic;
using System.Text;
using Optional;

namespace Rethought.Commands.Parser
{
    public class FuncParser<TInput, TOutput> : ITypeParser<TInput, TOutput>
    {
        private readonly Func<TInput, Option<TOutput>> func;

        public FuncParser(Func<TInput, Option<TOutput>> func)
        {
            this.func = func;
        }

        public Option<TOutput> Parse(TInput input)
        {
            return func.Invoke(input);
        }
    }
}
