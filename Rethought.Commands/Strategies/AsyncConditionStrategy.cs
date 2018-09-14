using System;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Conditions;
using Rethought.Commands.Conditions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Strategies
{
    public class AsyncConditionStrategy<TContext> : IStrategy<TContext>
    {
        private readonly IAsyncCondition<TContext> asyncCondition;

        public AsyncConditionStrategy(IAsyncCondition<TContext> asyncCondition)
        {
            this.asyncCondition = asyncCondition;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            if (!nextAsyncActionOption.TryGetValue(out var nextAsyncAction))
            {
                throw new ArgumentException($"There must be a succeeding {nameof(IAsyncAction<TContext>)}");
            }

            return new AsyncConditionalAsyncAction<TContext>(asyncCondition, nextAsyncAction);
        }
    }
}