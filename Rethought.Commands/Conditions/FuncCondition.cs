using System;

namespace Rethought.Commands.Conditions
{
    public class FuncCondition<TContext> : ICondition<TContext>
    {
        private readonly Func<TContext, ConditionResult> func;

        public FuncCondition(Func<TContext, ConditionResult> func)
        {
            this.func = func;
        }

        public ConditionResult Satisfied(TContext context)
        {
            return func.Invoke(context);
        }
    }
}