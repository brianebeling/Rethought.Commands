using System;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Conditions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class Condition<TContext> : IVisitor<TContext>
    {
        private readonly ICondition<TContext> condition;

        public Condition(ICondition<TContext> condition)
        {
            this.condition = condition;
        }

        public IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            if (!nextAsyncActionOption.TryGetValue(out var nextAsyncAction))
            {
                throw new ArgumentException($"There must be a succeeding {nameof(IAsyncResultFunc<TContext>)}");
            }

            return new Actions.Conditions.Condition<TContext>(condition, nextAsyncAction);
        }
    }
}