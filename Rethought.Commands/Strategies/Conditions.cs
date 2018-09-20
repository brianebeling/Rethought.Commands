using System;
using System.Collections.Generic;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Conditions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Strategies
{
    public class Conditions<TContext> : IStrategy<TContext>
    {
        private readonly IEnumerable<ICondition<TContext>> conditions;

        public Conditions(IEnumerable<ICondition<TContext>> conditions)
        {
            this.conditions = conditions;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            if (!nextAsyncActionOption.TryGetValue(out var nextAsyncAction))
            {
                throw new ArgumentException($"There must be a succeeding {nameof(IAsyncAction<TContext>)}");
            }

            return
                new Actions.Conditions.Condition<TContext>(
                    new AllOrFailed<TContext>(conditions),
                    nextAsyncAction);
        }
    }
}