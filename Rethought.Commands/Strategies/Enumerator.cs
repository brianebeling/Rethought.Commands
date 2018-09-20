using System.Collections.Generic;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Enumerating;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Strategies
{
    public class Enumerator<TContext> : IStrategy<TContext>
    {
        private readonly IEnumerable<IAsyncAction<TContext>> asyncActions;
        private readonly IFactory<TContext> factory;

        public Enumerator(IEnumerable<IAsyncAction<TContext>> asyncActions, IFactory<TContext> factory)
        {
            this.asyncActions = asyncActions;
            this.factory = factory;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            var enumeratingAsyncAction = factory.Create(asyncActions);

            return nextAsyncActionOption.TryGetValue(out var nextAsyncAction)
                ? Actions.Enumerating.Enumerator<TContext>.Create(enumeratingAsyncAction, nextAsyncAction)
                : enumeratingAsyncAction;
        }
    }
}