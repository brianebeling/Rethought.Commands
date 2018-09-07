using System;
using Optional;
using Rethought.Commands.Action;
using Rethought.Commands.Action.Conditions;
using Rethought.Commands.Conditions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class AsyncConditionVisitor<TContext> : IVisitor<TContext>
    {
        private readonly IAsyncCondition<TContext> asyncCondition;

        public AsyncConditionVisitor(IAsyncCondition<TContext> asyncCondition)
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