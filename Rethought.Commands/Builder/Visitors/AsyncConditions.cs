using System;
using System.Collections.Generic;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Conditions;
using Rethought.Commands.Conditions;
using Rethought.Commands.Conditions.Enumerator;
using Rethought.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class AsyncConditions<TContext> : Visitor<TContext>
    {
        private readonly IEnumerable<IAsyncCondition<TContext>> asyncConditions;

        public AsyncConditions(IEnumerable<IAsyncCondition<TContext>> asyncConditions)
        {
            this.asyncConditions = asyncConditions;
        }

        public override IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            if (!nextAsyncActionOption.TryGetValue(out var nextAsyncAction))
            {
                throw new ArgumentException($"There must be a succeeding {nameof(IAsyncResultFunc<TContext>)}");
            }

            return
                new AsyncResultCondition<TContext>(
                    new AsyncAllCondition<TContext>(asyncConditions),
                    nextAsyncAction);
        }
    }
}