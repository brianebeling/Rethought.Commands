using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ConditionResult Satisfied(TContext context)
        {
            var reasons = new List<Reason>();
            var stringBuilder = new StringBuilder();

            foreach (var condition in conditions)
            {
                var conditionResult = condition.Satisfied(context);

                if (!conditionResult.Satisfied && conditionResult.Reason.TryGetValue(out var reason))
                    reasons.AddRange(reason);
            }

            var value = stringBuilder.ToString();

            return !string.IsNullOrWhiteSpace(value)
                ? ConditionResult.FailWithReasons(new ReadOnlyCollection<Reason>(reasons))
                : ConditionResult.Success;
        }
    }
}