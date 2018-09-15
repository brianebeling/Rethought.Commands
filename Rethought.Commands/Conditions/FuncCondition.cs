using System;

namespace Rethought.Commands.Conditions
{
    public sealed class FuncCondition<TContext> : ICondition<TContext>
    {
        private readonly Func<TContext, ConditionResult> func;

        private FuncCondition(Func<TContext, ConditionResult> func)
        {
            this.func = func;
        }

        public ConditionResult Satisfied(TContext context)
        {
            return func.Invoke(context);
        }

        public static FuncCondition<TContext> Create(Func<TContext, ConditionResult> func)
        {
            return new FuncCondition<TContext>(func);
        }
    }
}