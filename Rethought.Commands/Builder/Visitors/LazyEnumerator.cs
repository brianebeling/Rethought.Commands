using System.Collections.Generic;
using System.Linq;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Enumerator;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class LazyEnumerator<TContext> : IStrategy<TContext>
    {
        private readonly IEnumerable<System.Func<IAsyncResultFunc<TContext>>> asyncActions;
        private readonly IFactory<TContext> factory;

        public LazyEnumerator(
            IEnumerable<System.Func<IAsyncResultFunc<TContext>>> asyncActions,
            IFactory<TContext> factory)
        {
            this.asyncActions = asyncActions;
            this.factory = factory;
        }

        public IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            var enumeratingAsyncAction = factory.Create(asyncActions.Select(x => x.Invoke()));

            return nextAsyncActionOption.TryGetValue(out var nextAsyncAction)
                ? Actions.Enumerator.Enumerator<TContext>.Create(enumeratingAsyncAction, nextAsyncAction)
                : enumeratingAsyncAction;
        }
    }
}