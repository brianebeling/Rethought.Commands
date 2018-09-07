using System;
using System.Collections.Generic;
using Rethought.Commands.Actions;
using Rethought.Commands.Conditions;
using Rethought.Commands.Parser;
using Rethought.Commands.Visitors;

namespace Rethought.Commands.Builder
{
    public static class AsyncActionBuilderExtensions
    {
        public static AsyncActionBuilder<TContext> WithConditions<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IEnumerable<ICondition<TContext>> conditions)
        {
            asyncActionBuilder.AddBuildingStep(new ConditionsVisitor<TContext>(conditions));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncConditions<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            IEnumerable<IAsyncCondition<TContext>> asyncConditions)
        {
            asyncActionBuilder.AddBuildingStep(new AsyncConditionsVisitor<TContext>(asyncConditions));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithCondition<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, ICondition<TContext> condition)
        {
            asyncActionBuilder.AddBuildingStep(new ConditionVisitor<TContext>(condition));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncCondition<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IAsyncCondition<TContext> asyncCondition)
        {
            asyncActionBuilder.AddBuildingStep(new AsyncConditionVisitor<TContext>(asyncCondition));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAdapter<TContext, TCommandSpecificContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            IAsyncTypeParser<TContext, TCommandSpecificContext> asyncTypeParser,
            Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
        {
            asyncActionBuilder.AddBuildingStep(
                new AsyncAdapterVisitor<TContext, TCommandSpecificContext>(asyncTypeParser, configuration));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAdapter<TContext, TCommandSpecificContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            ITypeParser<TContext, TCommandSpecificContext> typeParser,
            Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
        {
            asyncActionBuilder.AddBuildingStep(
                new AdapterVisitor<TContext, TCommandSpecificContext>(typeParser, configuration));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncAction<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IAsyncAction<TContext> asyncAction)
        {
            asyncActionBuilder.AddBuildingStep(new AsyncActionVisitor<TContext>(asyncAction));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAction<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IAction<TContext> asyncAction)
        {
            asyncActionBuilder.AddBuildingStep(new ActionVisitor<TContext>(asyncAction));

            return asyncActionBuilder;
        }
    }
}