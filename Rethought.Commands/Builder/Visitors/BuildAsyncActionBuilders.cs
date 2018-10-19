using System.Collections.Generic;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Enumerator;
using Rethought.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class BuildAsyncActionBuilders<TContext> : Visitor<TContext>
    {
        private readonly IEnumerable<System.Action<AsyncFuncBuilder<TContext>>> asyncActionBuilderActions;
        private readonly IEnumeratorFactory<TContext> enumeratorFactory;

        public BuildAsyncActionBuilders(
            IEnumerable<System.Action<AsyncFuncBuilder<TContext>>> asyncActionBuilderActions,
            IEnumeratorFactory<TContext> enumeratorFactory)
        {
            this.asyncActionBuilderActions = asyncActionBuilderActions;
            this.enumeratorFactory = enumeratorFactory;
        }

        public override IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            var asyncActions = new List<IAsyncResultFunc<TContext>>();

            foreach (var asyncActionBuilderAction in asyncActionBuilderActions)
            {
                var asyncActionBuilder = AsyncFuncBuilder<TContext>.Create();
                asyncActionBuilderAction.Invoke(asyncActionBuilder);
                asyncActions.Add(asyncActionBuilder.Build());
            }

            var enumeratingAsyncAction = enumeratorFactory.Create(asyncActions);

            return nextAsyncActionOption.TryGetValue(out var nextAsyncAction)
                ? Actions.Enumerator.Enumerator<TContext>.Create(enumeratingAsyncAction, nextAsyncAction)
                : enumeratingAsyncAction;
        }
    }
}