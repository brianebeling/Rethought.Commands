using System.Collections.Generic;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Enumerator;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class Enumerator<TContext> : IVisitor<TContext>
    {
        private readonly IEnumerable<IAsyncResultFunc<TContext>> asyncActions;
        private readonly IFactory<TContext> factory;

        public Enumerator(IEnumerable<IAsyncResultFunc<TContext>> asyncActions, IFactory<TContext> factory)
        {
            this.asyncActions = asyncActions;
            this.factory = factory;
        }

        public IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            var enumeratingAsyncAction = factory.Create(asyncActions);

            return nextAsyncActionOption.TryGetValue(out var nextAsyncAction)
                ? Actions.Enumerator.Enumerator<TContext>.Create(enumeratingAsyncAction, nextAsyncAction)
                : enumeratingAsyncAction;
        }
    }
}