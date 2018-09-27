using System;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Conditions;
using Rethought.Commands.Conditions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class AsyncCondition<TContext> : IStrategy<TContext>
    {
        private readonly IAsyncCondition<TContext> asyncCondition;

        public AsyncCondition(IAsyncCondition<TContext> asyncCondition)
        {
            this.asyncCondition = asyncCondition;
        }

        public IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            if (!nextAsyncActionOption.TryGetValue(out var nextAsyncAction))
            {
                throw new ArgumentException($"There must be a succeeding {nameof(IAsyncResultFunc<TContext>)}");
            }

            return new AsyncResultCondition<TContext>(asyncCondition, nextAsyncAction);
        }
    }
}