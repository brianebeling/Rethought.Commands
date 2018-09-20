using System.Collections.Generic;
using System.Linq;

namespace Rethought.Commands.Conditions
{
    public class AllOrFailed<TContext> : ICondition<TContext>
    {
        private readonly IEnumerable<ICondition<TContext>> conditions;

        public AllOrFailed(IEnumerable<ICondition<TContext>> conditions)
        {
            this.conditions = conditions;
        }

        public bool Satisfied(TContext context)
        {
            return conditions.All(condition => condition.Satisfied(context));
        }
    }
}