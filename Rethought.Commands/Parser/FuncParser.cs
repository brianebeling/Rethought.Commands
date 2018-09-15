using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace Rethought.Commands.Parser
{
    public sealed class FuncParser<TInput, TOutput> : ITypeParser<TInput, TOutput>
    {
        private readonly Func<TInput, Option<TOutput>> func;

        private FuncParser(Func<TInput, Option<TOutput>> func)
        {
            this.func = func;
        }

        public Option<TOutput> Parse(TInput input)
        {
            return func.Invoke(input);
        }

        public static FuncParser<TInput, TOutput> Create(
            Func<TInput, Option<TOutput>> func)
        {
            return new FuncParser<TInput, TOutput>(func);
        }
    }
}
