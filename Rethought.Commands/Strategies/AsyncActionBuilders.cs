using System.Collections.Generic;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Enumerating;
using Rethought.Commands.Builder;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Strategies
{
    public class AsyncActionBuilders<TContext> : IStrategy<TContext>
    {
        private readonly IEnumerable<System.Action<AsyncActionBuilder<TContext>>> asyncActionBuilderActions;
        private readonly IFactory<TContext> factory;

        public AsyncActionBuilders(
            IEnumerable<System.Action<AsyncActionBuilder<TContext>>> asyncActionBuilderActions,
            IFactory<TContext> factory)
        {
            this.asyncActionBuilderActions = asyncActionBuilderActions;
            this.factory = factory;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            var asyncActions = new List<IAsyncAction<TContext>>();

            foreach (var asyncActionBuilderAction in asyncActionBuilderActions)
            {
                var asyncActionBuilder = new AsyncActionBuilder<TContext>();
                asyncActionBuilderAction.Invoke(asyncActionBuilder);
                asyncActions.Add(asyncActionBuilder.Build());
            }

            var enumeratingAsyncAction = factory.Create(asyncActions);

            return nextAsyncActionOption.TryGetValue(out var nextAsyncAction)
                ? Actions.Enumerating.Enumerator<TContext>.Create(enumeratingAsyncAction, nextAsyncAction)
                : enumeratingAsyncAction;
        }
    }
}