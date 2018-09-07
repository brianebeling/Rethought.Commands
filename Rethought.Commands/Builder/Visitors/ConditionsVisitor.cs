using System;
using System.Collections.Generic;
using Optional;
using Rethought.Commands.Action;
using Rethought.Commands.Action.Conditions;
using Rethought.Commands.Conditions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class ConditionsVisitor<TContext> : IVisitor<TContext>
    {
        private readonly IEnumerable<ICondition<TContext>> conditions;

        public ConditionsVisitor(IEnumerable<ICondition<TContext>> conditions)
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
                new ConditionalAsyncAction<TContext>(
                    new AllOrFailureCondition<TContext>(conditions),
                    nextAsyncAction);
        }
    }
}