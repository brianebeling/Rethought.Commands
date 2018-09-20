using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Builder;
using Rethought.Commands.Parser;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Strategies
{
    public class Adapter<TContext, TCommandSpecificContext> : IStrategy<TContext>
    {
        private readonly System.Action<AsyncActionBuilder<TCommandSpecificContext>> configuration;
        private readonly ITypeParser<TContext, TCommandSpecificContext> parser;

        public Adapter(
            ITypeParser<TContext, TCommandSpecificContext> parser,
            System.Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
        {
            this.parser = parser;
            this.configuration = configuration;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            var asyncActionBuilder = new AsyncActionBuilder<TCommandSpecificContext>();
            configuration.Invoke(asyncActionBuilder);

            var command = asyncActionBuilder.Build();

            var asyncContextSwitchDecorator =
                new ContextAdapter<TContext, TCommandSpecificContext>(parser, command);

            if (nextAsyncActionOption.TryGetValue(out var nextAsyncAction))
            {
                return Actions.Enumerating.Enumerator<TContext>.Create(asyncContextSwitchDecorator, nextAsyncAction);
            }

            return asyncContextSwitchDecorator;
        }
    }
}