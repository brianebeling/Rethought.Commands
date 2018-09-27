using System;
using System.Collections.Generic;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Conditions;
using Rethought.Commands.Conditions.Enumerator;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class Conditions<TContext> : IStrategy<TContext>
    {
        private readonly IEnumerable<ICondition<TContext>> conditions;

        public Conditions(IEnumerable<ICondition<TContext>> conditions)
        {
            this.conditions = conditions;
        }

        public IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            if (!nextAsyncActionOption.TryGetValue(out var nextAsyncAction))
            {
                throw new ArgumentException($"There must be a succeeding {nameof(IAsyncResultFunc<TContext>)}");
            }

            return
                new Actions.Conditions.Condition<TContext>(
                    new All<TContext>(conditions),
                    nextAsyncAction);
        }
    }
}