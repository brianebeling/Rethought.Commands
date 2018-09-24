using System.Collections.Generic;
using System.Linq;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Enumerating;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Strategies
{
    public class LazyEnumerator<TContext> : IStrategy<TContext>
    {
        private readonly IEnumerable<System.Func<IAsyncAction<TContext>>> asyncActions;
        private readonly IFactory<TContext> factory;

        public LazyEnumerator(
            IEnumerable<System.Func<IAsyncAction<TContext>>> asyncActions,
            IFactory<TContext> factory)
        {
            this.asyncActions = asyncActions;
            this.factory = factory;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            var enumeratingAsyncAction = factory.Create(asyncActions.Select(x => x.Invoke()));

            return nextAsyncActionOption.TryGetValue(out var nextAsyncAction)
                ? Actions.Enumerating.Enumerator<TContext>.Create(enumeratingAsyncAction, nextAsyncAction)
                : enumeratingAsyncAction;
        }
    }
}