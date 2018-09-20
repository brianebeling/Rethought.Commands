using System;

namespace Rethought.Commands.Conditions
{
    public sealed class Func<TContext> : ICondition<TContext>
    {
        private readonly Func<TContext, bool> func;

        private Func(Func<TContext, bool> func)
        {
            this.func = func;
        }

        public bool Satisfied(TContext context)
        {
            return func.Invoke(context);
        }

        public static Func<TContext> Create(Func<TContext, bool> func)
        {
            return new Func<TContext>(func);
        }
    }
}