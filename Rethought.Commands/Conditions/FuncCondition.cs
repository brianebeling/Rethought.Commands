using System;

namespace Rethought.Commands.Conditions
{
    public sealed class FuncCondition<TContext> : ICondition<TContext>
    {
        private readonly Func<TContext, bool> func;

        private FuncCondition(Func<TContext, bool> func)
        {
            this.func = func;
        }

        public bool Satisfied(TContext context)
        {
            return func.Invoke(context);
        }

        public static FuncCondition<TContext> Create(Func<TContext, bool> func)
        {
            return new FuncCondition<TContext>(func);
        }
    }
}