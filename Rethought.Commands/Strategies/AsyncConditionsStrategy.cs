using System;
using System.Collections.Generic;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Conditions;
using Rethought.Commands.Conditions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Visitors
{
    public class AsyncConditionsStrategy<TContext> : IStrategy<TContext>
    {
        private readonly IEnumerable<IAsyncCondition<TContext>> asyncConditions;

        public AsyncConditionsStrategy(IEnumerable<IAsyncCondition<TContext>> asyncConditions)
        {
            this.asyncConditions = asyncConditions;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            if (!nextAsyncActionOption.TryGetValue(out var nextAsyncAction))
            {
                throw new ArgumentException($"There must be a succeeding {nameof(IAsyncAction<TContext>)}");
            }

            return
                new AsyncConditionalAsyncAction<TContext>(
                    new AsyncAllOrFailureCondition<TContext>(asyncConditions),
                    nextAsyncAction);
        }
    }
}