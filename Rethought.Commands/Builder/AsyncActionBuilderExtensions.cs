using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Adapter;
using Rethought.Commands.Actions.Enumerating;
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
            asyncActionBuilder.AddStrategy(new Conditions<TContext>(conditions));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithConditions<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IEnumerable<System.Func<TContext, bool>> conditions)
        {
            asyncActionBuilder.AddStrategy(
                new Conditions<TContext>(conditions.Select(Conditions.Func<TContext>.Create)));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncConditions<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            IEnumerable<IAsyncCondition<TContext>> asyncConditions)
        {
            asyncActionBuilder.AddStrategy(new AsyncConditions<TContext>(asyncConditions));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncConditions<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            IEnumerable<System.Func<TContext, Task<bool>>> asyncConditions)
        {
            asyncActionBuilder.AddStrategy(
                new AsyncConditions<TContext>(asyncConditions.Select(AsyncFunc<TContext>.Create)));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithCondition<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, ICondition<TContext> condition)
        {
            asyncActionBuilder.AddStrategy(new Condition<TContext>(condition));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithCondition<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, System.Func<TContext, bool> condition)
        {
            asyncActionBuilder.AddStrategy(new Condition<TContext>(Conditions.Func<TContext>.Create(condition)));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncCondition<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IAsyncCondition<TContext> asyncCondition)
        {
            asyncActionBuilder.AddStrategy(new AsyncCondition<TContext>(asyncCondition));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncCondition<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, System.Func<TContext, Task<bool>> asyncCondition)
        {
            asyncActionBuilder.AddStrategy(
                new AsyncCondition<TContext>(AsyncFunc<TContext>.Create(asyncCondition)));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAdapter<TContext, TCommandSpecificContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            IAsyncTypeParser<TContext, TCommandSpecificContext> asyncTypeParser,
            System.Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
        {
            asyncActionBuilder.AddStrategy(
                new AsyncAdapter<TContext, TCommandSpecificContext>(asyncTypeParser, configuration));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAdapter<TContext, TCommandSpecificContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            Func<TContext, CancellationToken, Task<Option<TCommandSpecificContext>>> asyncTypeParser,
            System.Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
        {
            asyncActionBuilder.AddStrategy(
                new AsyncAdapter<TContext, TCommandSpecificContext>(
                    AsyncFunc<TContext, TCommandSpecificContext>.Create(asyncTypeParser), configuration));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAdapter<TContext, TCommandSpecificContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            System.Func<TContext, Task<Option<TCommandSpecificContext>>> asyncTypeParser,
            System.Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
        {
            asyncActionBuilder.AddStrategy(
                new AsyncAdapter<TContext, TCommandSpecificContext>(
                    AsyncFunc<TContext, TCommandSpecificContext>.Create(asyncTypeParser), configuration));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAdapter<TContext, TCommandSpecificContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            ITypeParser<TContext, TCommandSpecificContext> typeParser,
            System.Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
        {
            asyncActionBuilder.AddStrategy(
                new Adapter<TContext, TCommandSpecificContext>(typeParser, configuration));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAdapter<TContext, TCommandSpecificContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            System.Func<TContext, Option<TCommandSpecificContext>> typeParser,
            System.Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
        {
            asyncActionBuilder.AddStrategy(
                new Adapter<TContext, TCommandSpecificContext>(
                    Parser.Func<TContext, TCommandSpecificContext>.Create(typeParser), configuration));
            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncAction<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IAsyncAction<TContext> asyncAction)
        {
            asyncActionBuilder.AddStrategy(new AsyncAction<TContext>(asyncAction));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncAction<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, Func<TContext, CancellationToken, Task> func)
        {
            asyncActionBuilder.AddStrategy(new AsyncAction<TContext>(FuncAdapter<TContext>.Create(func)));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAsyncAction<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, System.Func<TContext, Task> func)
        {
            asyncActionBuilder.AddStrategy(new AsyncAction<TContext>(FuncAdapter<TContext>.Create(func)));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAction<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IAction<TContext> action)
        {
            asyncActionBuilder.AddStrategy(new Strategies.Action<TContext>(action));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithAction<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, System.Action<TContext> action)
        {
            asyncActionBuilder.AddStrategy(new Strategies.Action<TContext>(ActionAdapter<TContext>.Create(action)));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithEnumerating<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IEnumerable<IAsyncAction<TContext>> asyncActions,
            IFactory<TContext> factory)
        {
            asyncActionBuilder.AddStrategy(new Strategies.Enumerator<TContext>(asyncActions, factory));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithEnumerating<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            IEnumerable<System.Func<IAsyncAction<TContext>>> asyncActions, IFactory<TContext> factory)
        {
            asyncActionBuilder.AddStrategy(new LazyEnumerator<TContext>(asyncActions, factory));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> WithEnumerating<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            IEnumerable<System.Action<AsyncActionBuilder<TContext>>> configuration, IFactory<TContext> factory)
        {
            asyncActionBuilder.AddStrategy(new AsyncActionBuilders<TContext>(configuration, factory));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> Any<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            IEnumerable<System.Action<AsyncActionBuilder<TContext>>> configuration)
        {
            asyncActionBuilder.AddStrategy(new AsyncActionBuilders<TContext>(configuration,
                new AnyOrFailedFactory<TContext>()));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> Any<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            IEnumerable<System.Func<IAsyncAction<TContext>>> asyncActions)
        {
            asyncActionBuilder.AddStrategy(new LazyEnumerator<TContext>(asyncActions,
                new AnyOrFailedFactory<TContext>()));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> Any<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IEnumerable<IAsyncAction<TContext>> asyncActions)
        {
            asyncActionBuilder.AddStrategy(new Strategies.Enumerator<TContext>(asyncActions,
                new AnyOrFailedFactory<TContext>()));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> All<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            IEnumerable<System.Action<AsyncActionBuilder<TContext>>> configuration, bool shortCircuiting = true)
        {
            IFactory<TContext> factory;

            if (shortCircuiting)
                factory = new AllOrFailedFactory<TContext>();
            else
                factory = new PersistingAllOrFailedFactory<TContext>();

            asyncActionBuilder.AddStrategy(new AsyncActionBuilders<TContext>(configuration, factory));

            return asyncActionBuilder;
        }

        /// <summary>
        ///     Sets a prototype. A prototype is inserted as the first strategy.
        ///     Use this if you want to extend an existing <see cref="IAsyncAction{TContext}" />.
        /// </summary>
        /// <param name="asyncActionBuilder">The async action builder</param>
        /// <param name="asyncAction">The asynchronous action.</param>
        /// <returns></returns>
        public static AsyncActionBuilder<TContext> WithPrototype<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IAsyncAction<TContext> asyncAction)
        {
            asyncActionBuilder.Strategies.Insert(0, new Prototype<TContext>(asyncAction));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> All<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder,
            IEnumerable<System.Func<IAsyncAction<TContext>>> asyncActions, bool shortCircuiting = true)
        {
            IFactory<TContext> factory;

            if (shortCircuiting)
                factory = new AllOrFailedFactory<TContext>();
            else
                factory = new PersistingAllOrFailedFactory<TContext>();

            asyncActionBuilder.AddStrategy(new LazyEnumerator<TContext>(asyncActions, factory));

            return asyncActionBuilder;
        }

        public static AsyncActionBuilder<TContext> All<TContext>(
            this AsyncActionBuilder<TContext> asyncActionBuilder, IEnumerable<IAsyncAction<TContext>> asyncActions,
            bool shortCircuiting = true)
        {
            IFactory<TContext> factory;

            if (shortCircuiting)
                factory = new AllOrFailedFactory<TContext>();
            else
                factory = new PersistingAllOrFailedFactory<TContext>();

            asyncActionBuilder.AddStrategy(new Strategies.Enumerator<TContext>(asyncActions, factory));

            return asyncActionBuilder;
        }
    }
}