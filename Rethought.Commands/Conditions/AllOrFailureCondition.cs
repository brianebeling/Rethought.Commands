using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Conditions
{
    public class AllOrFailureCondition<TContext> : ICondition<TContext>
    {
        private readonly IEnumerable<ICondition<TContext>> conditions;

        public AllOrFailureCondition(IEnumerable<ICondition<TContext>> conditions)
        {
            this.conditions = conditions;
        }

        public bool Satisfied(TContext context)
        {
            return conditions.All(condition => condition.Satisfied(context));
        }
    }
}