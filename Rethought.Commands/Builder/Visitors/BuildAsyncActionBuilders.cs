using System.Collections.Generic;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Enumerator;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class BuildAsyncActionBuilders<TContext> : IStrategy<TContext>
    {
        private readonly IEnumerable<System.Action<AsyncFuncBuilder<TContext>>> asyncActionBuilderActions;
        private readonly IFactory<TContext> factory;

        public BuildAsyncActionBuilders(
            IEnumerable<System.Action<AsyncFuncBuilder<TContext>>> asyncActionBuilderActions,
            IFactory<TContext> factory)
        {
            this.asyncActionBuilderActions = asyncActionBuilderActions;
            this.factory = factory;
        }

        public IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            var asyncActions = new List<IAsyncResultFunc<TContext>>();

            foreach (var asyncActionBuilderAction in asyncActionBuilderActions)
            {
                var asyncActionBuilder = AsyncFuncBuilder<TContext>.Create();
                asyncActionBuilderAction.Invoke(asyncActionBuilder);
                asyncActions.Add(asyncActionBuilder.Build());
            }

            var enumeratingAsyncAction = factory.Create(asyncActions);

            return nextAsyncActionOption.TryGetValue(out var nextAsyncAction)
                ? Actions.Enumerator.Enumerator<TContext>.Create(enumeratingAsyncAction, nextAsyncAction)
                : enumeratingAsyncAction;
        }
    }
}