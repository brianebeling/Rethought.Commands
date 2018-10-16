using System.Collections.Generic;
using System.Linq;

namespace Rethought.Commands.Conditions.Enumerator
{
    public class AllCondition<TContext> : ICondition<TContext>
    {
        private readonly IEnumerable<ICondition<TContext>> conditions;

        public AllCondition(IEnumerable<ICondition<TContext>> conditions)
        {
            this.conditions = conditions;
        }

        public bool Satisfied(TContext context)
        {
            return conditions.All(condition => condition.Satisfied(context));
        }
    }
}