using System;
using System.Collections.Generic;
using Optional;
using Rethought.Commands.Action;
using Rethought.Commands.Action.Conditions;
using Rethought.Commands.Conditions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class AsyncConditionsVisitor<TContext> : IVisitor<TContext>
    {
        private readonly IEnumerable<IAsyncCondition<TContext>> asyncConditions;

        public AsyncConditionsVisitor(IEnumerable<IAsyncCondition<TContext>> asyncConditions)
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