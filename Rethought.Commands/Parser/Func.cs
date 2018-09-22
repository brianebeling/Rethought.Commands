﻿using Optional;

namespace Rethought.Commands.Parser
{
    public sealed class Func<TInput, TOutput> : ITypeParser<TInput, TOutput>
    {
        private readonly System.Func<TInput, Option<TOutput, bool>> func;

        private Func(System.Func<TInput, Option<TOutput, bool>> func)
        {
            this.func = func;
        }

        public Option<TOutput, bool> Parse(TInput input)
        {
            return func.Invoke(input);
        }

        public static Func<TInput, TOutput> Create(System.Func<TInput, Option<TOutput, bool>> func)
        {
            return new Func<TInput, TOutput>(func);
        }
    }
}