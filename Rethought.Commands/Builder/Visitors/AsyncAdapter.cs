using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Adapters.Context.AsyncResultFunc;
using Rethought.Commands.Parser;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class AsyncAdapter<TInput, TOutput> : IVisitor<TInput>
    {
        private readonly IAsyncAbortableTypeParser<TInput, TOutput> asyncAbortableParser;
        private readonly System.Action<AsyncFuncBuilder<TOutput>> configuration;

        public AsyncAdapter(
            IAsyncAbortableTypeParser<TInput, TOutput> asyncAbortableParser,
            System.Action<AsyncFuncBuilder<TOutput>> configuration)
        {
            this.asyncAbortableParser = asyncAbortableParser;
            this.configuration = configuration;
        }

        public IAsyncResultFunc<TInput> Invoke(Option<IAsyncResultFunc<TInput>> nextAsyncActionOption)
        {
            var asyncActionBuilder = AsyncFuncBuilder<TOutput>.Create();
            configuration.Invoke(asyncActionBuilder);

            var command = asyncActionBuilder.Build();

            var asyncContextSwitchDecorator =
                new AsyncResultFunc<TInput, TOutput>(asyncAbortableParser, command);

            if (nextAsyncActionOption.TryGetValue(out var nextAsyncAction))
            {
                return Actions.Enumerator.Enumerator<TInput>.Create(asyncContextSwitchDecorator, nextAsyncAction);
            }

            return asyncContextSwitchDecorator;
        }
    }
}