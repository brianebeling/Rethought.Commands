using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Adapters.Context.AsyncResultFunc;
using Rethought.Commands.Parser;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class AsyncAdapter<TInput, TOutput> : IStrategy<TInput>
    {
        private readonly IAsyncTypeParser<TInput, TOutput> asyncParser;
        private readonly System.Action<AsyncFuncBuilder<TOutput>> configuration;

        public AsyncAdapter(
            IAsyncTypeParser<TInput, TOutput> asyncParser,
            System.Action<AsyncFuncBuilder<TOutput>> configuration)
        {
            this.asyncParser = asyncParser;
            this.configuration = configuration;
        }

        public IAsyncResultFunc<TInput> Invoke(Option<IAsyncResultFunc<TInput>> nextAsyncActionOption)
        {
            var asyncActionBuilder = AsyncFuncBuilder<TOutput>.Create();
            configuration.Invoke(asyncActionBuilder);

            var command = asyncActionBuilder.Build();

            var asyncContextSwitchDecorator =
                new AsyncResultFunc<TInput, TOutput>(asyncParser, command);

            if (nextAsyncActionOption.TryGetValue(out var nextAsyncAction))
            {
                return Actions.Enumerator.Enumerator<TInput>.Create(asyncContextSwitchDecorator, nextAsyncAction);
            }

            return asyncContextSwitchDecorator;
        }
    }
}