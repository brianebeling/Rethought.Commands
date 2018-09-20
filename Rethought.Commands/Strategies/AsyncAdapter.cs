using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Builder;
using Rethought.Commands.Parser;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Strategies
{
    public class AsyncAdapter<TContext, TCommandSpecificContext> : IStrategy<TContext>
    {
        private readonly IAsyncTypeParser<TContext, TCommandSpecificContext> asyncParser;
        private readonly System.Action<AsyncActionBuilder<TCommandSpecificContext>> configuration;

        public AsyncAdapter(
            IAsyncTypeParser<TContext, TCommandSpecificContext> asyncParser,
            System.Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
        {
            this.asyncParser = asyncParser;
            this.configuration = configuration;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            var asyncActionBuilder = new AsyncActionBuilder<TCommandSpecificContext>();
            configuration.Invoke(asyncActionBuilder);

            var command = asyncActionBuilder.Build();

            var asyncContextSwitchDecorator =
                new AsyncContextAdapter<TContext, TCommandSpecificContext>(asyncParser, command);

            if (nextAsyncActionOption.TryGetValue(out var nextAsyncAction))
            {
                return Actions.Enumerating.Enumerator<TContext>.Create(asyncContextSwitchDecorator, nextAsyncAction);
            }

            return asyncContextSwitchDecorator;
        }
    }
}