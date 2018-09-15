using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Adapter;
using Rethought.Commands.Conditions;
using Rethought.Commands.Parser;
using Rethought.Commands.Strategies;

namespace Rethought.Commands.Builder
{
    public static class AsyncActionBuilderExtensions
    {
        public static AsyncActionBuilder<TContext> WithConditions<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IEnumerable<ICondition<TContext>> conditions)
        {
            asyncActionBuilder.AddStrategy(new ConditionsStrategy<TContext>(conditions));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithConditions<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IEnumerable<Func<TContext, bool>> conditions)
        {
            asyncActionBuilder.AddStrategy(new ConditionsStrategy<TContext>(conditions.Select(FuncCondition<TContext>.Create)));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncConditions<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            IEnumerable<IAsyncCondition<TContext>> asyncConditions)
        {
            asyncActionBuilder.AddStrategy(new AsyncConditionsStrategy<TContext>(asyncConditions));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncConditions<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            IEnumerable<Func<TContext, Task<bool>>> asyncConditions)
        {
            asyncActionBuilder.AddStrategy(new AsyncConditionsStrategy<TContext>(asyncConditions.Select(AsyncFuncCondition<TContext>.Create)));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithCondition<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, ICondition<TContext> condition)
        {
            asyncActionBuilder.AddStrategy(new ConditionStrategy<TContext>(condition));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithCondition<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, Func<TContext, bool> condition)
        {
            asyncActionBuilder.AddStrategy(new ConditionStrategy<TContext>(FuncCondition<TContext>.Create(condition)));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncCondition<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IAsyncCondition<TContext> asyncCondition)
        {
            asyncActionBuilder.AddStrategy(new AsyncConditionStrategy<TContext>(asyncCondition));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncCondition<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, Func<TContext, Task<bool>> asyncCondition)
        {
            asyncActionBuilder.AddStrategy(new AsyncConditionStrategy<TContext>(AsyncFuncCondition<TContext>.Create(asyncCondition)));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAdapter<TContext, TCommandSpecificContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            IAsyncTypeParser<TContext, TCommandSpecificContext> asyncTypeParser,
            Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
        {
            asyncActionBuilder.AddStrategy(
                new AsyncAdapterStrategy<TContext, TCommandSpecificContext>(asyncTypeParser, configuration));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAdapter<TContext, TCommandSpecificContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            Func<TContext, CancellationToken, Task<Option<TCommandSpecificContext>>> asyncTypeParser,
            Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
        {
            asyncActionBuilder.AddStrategy(
                new AsyncAdapterStrategy<TContext, TCommandSpecificContext>(AsyncFuncParser<TContext, TCommandSpecificContext>.Create(asyncTypeParser), configuration));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAdapter<TContext, TCommandSpecificContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            Func<TContext, Task<Option<TCommandSpecificContext>>> asyncTypeParser,
            Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
        {
            asyncActionBuilder.AddStrategy(
                new AsyncAdapterStrategy<TContext, TCommandSpecificContext>(AsyncFuncParser<TContext, TCommandSpecificContext>.Create(asyncTypeParser), configuration));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAdapter<TContext, TCommandSpecificContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            ITypeParser<TContext, TCommandSpecificContext> typeParser,
            Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
        {
            asyncActionBuilder.AddStrategy(
                new AdapterStrategy<TContext, TCommandSpecificContext>(typeParser, configuration));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAdapter<TContext, TCommandSpecificContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            Func<TContext, Option<TCommandSpecificContext>> typeParser,
            Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
        {
            asyncActionBuilder.AddStrategy(
                new AdapterStrategy<TContext, TCommandSpecificContext>(FuncParser<TContext, TCommandSpecificContext>.Create(typeParser), configuration));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncAction<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IAsyncAction<TContext> asyncAction)
        {
            asyncActionBuilder.AddStrategy(new AsyncActionStrategy<TContext>(asyncAction));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncAction<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, Func<TContext, CancellationToken, Task> func)
        {
            asyncActionBuilder.AddStrategy(new AsyncActionStrategy<TContext>(FuncAdapter<TContext>.Create(func)));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncAction<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, Func<TContext, Task> func)
        {
            asyncActionBuilder.AddStrategy(new AsyncActionStrategy<TContext>(FuncAdapter<TContext>.Create(func)));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAction<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IAction<TContext> action)
        {
            asyncActionBuilder.AddStrategy(new ActionStrategy<TContext>(action));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAction<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, Action<TContext> action)
        {
            asyncActionBuilder.AddStrategy(new ActionStrategy<TContext>(ActionAdapter<TContext>.Create(action)));

            return asyncActionBuilder;
        }
    }
}