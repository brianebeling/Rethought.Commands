using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Adapters.AsyncFunc;
using Rethought.Commands.Actions.Adapters.System.Action;
using Rethought.Commands.Actions.Adapters.System.Func;
using Rethought.Commands.Actions.Enumerator;
using Rethought.Commands.Builder.Visitors;
using Rethought.Commands.Conditions;
using Rethought.Commands.Parser;
using Rethought.Optional;

namespace Rethought.Commands.Builder
{
    public static class AsyncActionBuilderExtensions
    {
        public static AsyncFuncBuilder<TContext> WithConditions<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IEnumerable<ICondition<TContext>> conditions)
        {
            asyncFuncBuilder.AddStrategy(new Conditions<TContext>(conditions));
            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithConditions<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IEnumerable<System.Func<TContext, bool>> conditions)
        {
            asyncFuncBuilder.AddStrategy(
                new Conditions<TContext>(conditions.Select(Conditions.FuncCondition<TContext>.Create)));
            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithAsyncConditions<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IEnumerable<IAsyncCondition<TContext>> asyncConditions)
        {
            asyncFuncBuilder.AddStrategy(new AsyncConditions<TContext>(asyncConditions));
            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithAsyncConditions<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IEnumerable<System.Func<TContext, Task<bool>>> asyncConditions)
        {
            asyncFuncBuilder.AddStrategy(
                new AsyncConditions<TContext>(asyncConditions.Select(AsyncFuncCondition<TContext>.Create)));
            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithCondition<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            ICondition<TContext> condition)
        {
            asyncFuncBuilder.AddStrategy(new Condition<TContext>(condition));
            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithCondition<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            System.Func<TContext, bool> condition)
        {
            asyncFuncBuilder.AddStrategy(new Condition<TContext>(Conditions.FuncCondition<TContext>.Create(condition)));
            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithAsyncCondition<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IAsyncCondition<TContext> asyncCondition)
        {
            asyncFuncBuilder.AddStrategy(new AsyncCondition<TContext>(asyncCondition));
            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithAsyncCondition<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            System.Func<TContext, Task<bool>> asyncCondition)
        {
            asyncFuncBuilder.AddStrategy(
                new AsyncCondition<TContext>(AsyncFuncCondition<TContext>.Create(asyncCondition)));
            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TInput> WithAdapter<TInput, TOutput>(
            this AsyncFuncBuilder<TInput> asyncFuncBuilder,
            IAsyncAbortableTypeParser<TInput, TOutput> asyncAbortableTypeParser,
            System.Action<AsyncFuncBuilder<TOutput>> configuration)
        {
            asyncFuncBuilder.AddStrategy(
                new AsyncAdapter<TInput, TOutput>(asyncAbortableTypeParser, configuration));
            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TInput> WithAdapter<TInput, TOutput>(
            this AsyncFuncBuilder<TInput> asyncFuncBuilder,
            Func<TInput, CancellationToken, Task<(bool, Option<TOutput>)>> asyncTypeParser,
            System.Action<AsyncFuncBuilder<TOutput>> configuration)
        {
            asyncFuncBuilder.AddStrategy(
                new AsyncAdapter<TInput, TOutput>(
                    AsyncAbortableFunc<TInput, TOutput>.Create(asyncTypeParser),
                    configuration));
            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TInput> WithAdapter<TInput, TOutput>(
            this AsyncFuncBuilder<TInput> asyncFuncBuilder,
            System.Func<TInput, Task<(bool, Option<TOutput>)>> asyncTypeParser,
            System.Action<AsyncFuncBuilder<TOutput>> configuration)
        {
            asyncFuncBuilder.AddStrategy(
                new AsyncAdapter<TInput, TOutput>(
                    AsyncAbortableFunc<TInput, TOutput>.Create(asyncTypeParser),
                    configuration));
            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TInput> WithAdapter<TInput, TOutput>(
            this AsyncFuncBuilder<TInput> asyncFuncBuilder,
            IAbortableTypeParser<TInput, TOutput> abortableTypeParser,
            System.Action<AsyncFuncBuilder<TOutput>> configuration)
        {
            asyncFuncBuilder.AddStrategy(
                new AbortableAdapter<TInput, TOutput>(abortableTypeParser, configuration));
            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TInput> WithAdapter<TInput, TOutput>(
            this AsyncFuncBuilder<TInput> asyncFuncBuilder,
            ITypeParser<TInput, TOutput> typeParser,
            System.Action<AsyncFuncBuilder<TOutput>> configuration)
        {
            asyncFuncBuilder.AddStrategy(
                new Adapter<TInput, TOutput>(typeParser, configuration));
            return asyncFuncBuilder;
        }


        public static AsyncFuncBuilder<TInput> WithAdapter<TInput, TOutput>(
            this AsyncFuncBuilder<TInput> asyncFuncBuilder,
            System.Func<TInput, (bool, Option<TOutput>)> typeParser,
            System.Action<AsyncFuncBuilder<TOutput>> configuration)
        {
            asyncFuncBuilder.AddStrategy(
                new AbortableAdapter<TInput, TOutput>(
                    AbortableFunc<TInput, TOutput>.Create(typeParser),
                    configuration));
            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TInput> WithAdapter<TInput, TOutput>(
            this AsyncFuncBuilder<TInput> asyncFuncBuilder,
            System.Func<TInput, Option<TOutput>> typeParser,
            System.Action<AsyncFuncBuilder<TOutput>> configuration)
        {
            asyncFuncBuilder.AddStrategy(
                new Adapter<TInput, TOutput>(
                    Parser.Func<TInput, TOutput>.Create(typeParser),
                    configuration));
            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithAsyncFunc<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IAsyncResultFunc<TContext> asyncResultFunc)
        {
            asyncFuncBuilder.AddStrategy(new AsyncResultFunc<TContext>(asyncResultFunc));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithAsyncFunc<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            Func<TContext, CancellationToken, Task<Result>> asyncFunc)
        {
            asyncFuncBuilder.AddStrategy(
                new AsyncResultFunc<TContext>(asyncFunc.ToAsyncResultFunc()));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithAsyncFunc<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            System.Func<TContext, Task> asyncFunc)
        {
            asyncFuncBuilder.AddStrategy(
                new AsyncResultFunc<TContext>(asyncFunc.ToAsyncFunc().ToAsyncResultFunc()));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithAsyncFunc<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IAsyncFunc<TContext> asyncFunc)
        {
            asyncFuncBuilder.AddStrategy(
                new AsyncResultFunc<TContext>(asyncFunc.ToAsyncResultFunc()));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithAsyncFunc<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            Func<TContext, CancellationToken, Task> asyncFunc)
        {
            asyncFuncBuilder.AddStrategy(
                new AsyncResultFunc<TContext>(asyncFunc.ToAsyncFunc().ToAsyncResultFunc()));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithAsyncFunc<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            System.Func<TContext, Task<Result>> asyncFunc)
        {
            asyncFuncBuilder.AddStrategy(
                new AsyncResultFunc<TContext>(asyncFunc.ToAsyncResultFunc()));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithFunc<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IResultFunc<TContext> resultFunc)
        {
            asyncFuncBuilder.AddStrategy(new Visitors.Func<TContext>(resultFunc));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithFunc<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            System.Func<TContext, Result> func)
        {
            asyncFuncBuilder.AddStrategy(new Visitors.Func<TContext>(func.ToResultFunc()));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithAction<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            System.Action<TContext> action)
        {
            asyncFuncBuilder.AddStrategy(new Visitors.Func<TContext>(action.ToResultFunc()));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithAction<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IAction<TContext> action)
        {
            asyncFuncBuilder.AddStrategy(new Visitors.Action<TContext>(action));

            return asyncFuncBuilder;
        }


        public static AsyncFuncBuilder<TContext> WithEnumerating<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IEnumerable<IAsyncResultFunc<TContext>> asyncActions,
            IEnumeratorFactory<TContext> enumeratorFactory)
        {
            asyncFuncBuilder.AddStrategy(new Visitors.Enumerator<TContext>(asyncActions, enumeratorFactory));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithEnumerating<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IEnumerable<System.Func<IAsyncResultFunc<TContext>>> asyncActions,
            IEnumeratorFactory<TContext> enumeratorFactory)
        {
            asyncFuncBuilder.AddStrategy(new LazyEnumerator<TContext>(asyncActions, enumeratorFactory));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> WithEnumerating<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IEnumerable<System.Action<AsyncFuncBuilder<TContext>>> configuration,
            IEnumeratorFactory<TContext> enumeratorFactory)
        {
            asyncFuncBuilder.AddStrategy(new BuildAsyncActionBuilders<TContext>(configuration, enumeratorFactory));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> Any<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IEnumerable<System.Action<AsyncFuncBuilder<TContext>>> configuration,
            System.Func<Result, bool> predicate)
        {
            asyncFuncBuilder.AddStrategy(
                new BuildAsyncActionBuilders<TContext>(
                    configuration,
                    new AnyEnumeratorFactory<TContext>(predicate)));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> Any<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IEnumerable<System.Func<IAsyncResultFunc<TContext>>> asyncActions,
            System.Func<Result, bool> predicate)
        {
            asyncFuncBuilder.AddStrategy(
                new LazyEnumerator<TContext>(
                    asyncActions,
                    new AnyEnumeratorFactory<TContext>(predicate)));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> Any<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IEnumerable<IAsyncResultFunc<TContext>> asyncActions,
            System.Func<Result, bool> predicate)
        {
            asyncFuncBuilder.AddStrategy(
                new Visitors.Enumerator<TContext>(
                    asyncActions,
                    new AnyEnumeratorFactory<TContext>(predicate)));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> All<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IEnumerable<System.Action<AsyncFuncBuilder<TContext>>> configuration,
            System.Func<Result, bool> predicate,
            bool shortCircuiting = true)
        {
            IEnumeratorFactory<TContext> enumeratorFactory;

            if (shortCircuiting)
                enumeratorFactory = new AllEnumeratorEnumeratorFactory<TContext>(predicate);
            else
                enumeratorFactory = new ContinuingAllEnumeratorFactory<TContext>(predicate);

            asyncFuncBuilder.AddStrategy(new BuildAsyncActionBuilders<TContext>(configuration, enumeratorFactory));

            return asyncFuncBuilder;
        }

        /// <summary>
        ///     Sets a prototype. A prototype is inserted as the first strategy.
        ///     Use this if you want to extend an existing <see cref="IAsyncResultFunc{TContext}" />.
        /// </summary>
        /// <param name="asyncFuncBuilder">The async action builder</param>
        /// <param name="asyncResultFunc">The asynchronous action.</param>
        /// <returns></returns>
        public static AsyncFuncBuilder<TContext> WithPrototype<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IAsyncResultFunc<TContext> asyncResultFunc)
        {
            asyncFuncBuilder.Visitors.Insert(0, new Prototype<TContext>(asyncResultFunc));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> All<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IEnumerable<System.Func<IAsyncResultFunc<TContext>>> asyncActions,
            System.Func<Result, bool> predicate,
            bool shortCircuiting = true)
        {
            IEnumeratorFactory<TContext> enumeratorFactory;

            if (shortCircuiting)
                enumeratorFactory = new AllEnumeratorEnumeratorFactory<TContext>(predicate);
            else
                enumeratorFactory = new ContinuingAllEnumeratorFactory<TContext>(predicate);

            asyncFuncBuilder.AddStrategy(new LazyEnumerator<TContext>(asyncActions, enumeratorFactory));

            return asyncFuncBuilder;
        }

        public static AsyncFuncBuilder<TContext> All<TContext>(
            this AsyncFuncBuilder<TContext> asyncFuncBuilder,
            IEnumerable<IAsyncResultFunc<TContext>> asyncActions,
            System.Func<Result, bool> predicate,
            bool shortCircuiting = true)
        {
            IEnumeratorFactory<TContext> enumeratorFactory;

            if (shortCircuiting)
                enumeratorFactory = new AllEnumeratorEnumeratorFactory<TContext>(predicate);
            else
                enumeratorFactory = new ContinuingAllEnumeratorFactory<TContext>(predicate);

            asyncFuncBuilder.AddStrategy(new Visitors.Enumerator<TContext>(asyncActions, enumeratorFactory));

            return asyncFuncBuilder;
        }
    }
}