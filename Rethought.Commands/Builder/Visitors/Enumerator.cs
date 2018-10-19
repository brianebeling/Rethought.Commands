using System.Collections.Generic;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Enumerator;
using Rethought.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class Enumerator<TContext> : Visitor<TContext>
    {
        private readonly IEnumerable<IAsyncResultFunc<TContext>> asyncActions;
        private readonly IEnumeratorFactory<TContext> enumeratorFactory;

        public Enumerator(IEnumerable<IAsyncResultFunc<TContext>> asyncActions, IEnumeratorFactory<TContext> enumeratorFactory)
        {
            this.asyncActions = asyncActions;
            this.enumeratorFactory = enumeratorFactory;
        }

        public override IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            var enumeratingAsyncAction = enumeratorFactory.Create(asyncActions);

            return nextAsyncActionOption.TryGetValue(out var nextAsyncAction)
                ? Actions.Enumerator.Enumerator<TContext>.Create(enumeratingAsyncAction, nextAsyncAction)
                : enumeratingAsyncAction;
        }
    }
}