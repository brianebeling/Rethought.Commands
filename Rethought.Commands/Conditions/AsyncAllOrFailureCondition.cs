using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Conditions
{
    public class AsyncAllOrFailureCondition<TContext> : IAsyncCondition<TContext>
    {
        private readonly IEnumerable<IAsyncCondition<TContext>> asyncConditions;

        public AsyncAllOrFailureCondition(IEnumerable<IAsyncCondition<TContext>> asyncConditions)
        {
            this.asyncConditions = asyncConditions;
        }

        public async Task<ConditionResult> SatisfiedAsync(TContext context)
        {
            var reasons = new List<Reason>();
            var stringBuilder = new StringBuilder();

            foreach (var asyncCondition in asyncConditions)
            {
                var conditionResult = await asyncCondition.SatisfiedAsync(context);

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