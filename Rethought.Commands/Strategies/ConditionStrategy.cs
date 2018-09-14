﻿using System;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Conditions;
using Rethought.Commands.Conditions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Visitors
{
    public class ConditionStrategy<TContext> : IStrategy<TContext>
    {
        private readonly ICondition<TContext> condition;

        public ConditionStrategy(ICondition<TContext> condition)
        {
            this.condition = condition;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            if (!nextAsyncActionOption.TryGetValue(out var nextAsyncAction))
            {
                throw new ArgumentException($"There must be a succeeding {nameof(IAsyncAction<TContext>)}");
            }

            return new ConditionalAsyncAction<TContext>(condition, nextAsyncAction);
        }
    }
}