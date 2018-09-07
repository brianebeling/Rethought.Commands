using System;
using Optional;
using Rethought.Commands.Action;
using Rethought.Commands.Parser;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class AsyncAdapterVisitor<TContext, TCommandSpecificContext> : IVisitor<TContext>
    {
        private readonly IAsyncTypeParser<TContext, TCommandSpecificContext> asyncParser;
        private readonly Action<AsyncActionBuilder<TCommandSpecificContext>> configuration;

        public AsyncAdapterVisitor(
            IAsyncTypeParser<TContext, TCommandSpecificContext> asyncParser,
            Action<AsyncActionBuilder<TCommandSpecificContext>> configuration)
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
                new AsyncContextAdapterAsyncActionDecorator<TContext, TCommandSpecificContext>(asyncParser, command);

            if (nextAsyncActionOption.TryGetValue(out var nextAsyncAction))
            {
                return EnumeratingAsyncAction<TContext>.Create(asyncContextSwitchDecorator, nextAsyncAction);
            }

            return asyncContextSwitchDecorator;
        }
    }
}