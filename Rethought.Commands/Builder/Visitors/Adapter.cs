using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Parser;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class Adapter<TInput, TOutput> : IStrategy<TInput>
    {
        private readonly System.Action<AsyncFuncBuilder<TOutput>> configuration;
        private readonly ITypeParser<TInput, TOutput> parser;

        public Adapter(
            ITypeParser<TInput, TOutput> parser,
            System.Action<AsyncFuncBuilder<TOutput>> configuration)
        {
            this.parser = parser;
            this.configuration = configuration;
        }

        public IAsyncResultFunc<TInput> Invoke(Option<IAsyncResultFunc<TInput>> nextAsyncActionOption)
        {
            var asyncActionBuilder = AsyncFuncBuilder<TOutput>.Create();
            configuration.Invoke(asyncActionBuilder);

            var command = asyncActionBuilder.Build();

            var asyncContextSwitchDecorator =
                new ContextAdapter<TInput, TOutput>(parser, command);

            if (nextAsyncActionOption.TryGetValue(out var nextAsyncAction))
            {
                return Actions.Enumerator.Enumerator<TInput>.Create(asyncContextSwitchDecorator, nextAsyncAction);
            }

            return asyncContextSwitchDecorator;
        }
    }
}