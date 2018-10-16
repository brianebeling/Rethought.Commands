using System.Collections.Generic;
using System.Linq;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Enumerator;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class LazyEnumerator<TContext> : Visitor<TContext>
    {
        private readonly IEnumerable<System.Func<IAsyncResultFunc<TContext>>> asyncActions;
        private readonly IEnumeratorFactory<TContext> enumeratorFactory;

        public LazyEnumerator(
            IEnumerable<System.Func<IAsyncResultFunc<TContext>>> asyncActions,
            IEnumeratorFactory<TContext> enumeratorFactory)
        {
            this.asyncActions = asyncActions;
            this.enumeratorFactory = enumeratorFactory;
        }

        public override IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            var enumeratingAsyncAction = enumeratorFactory.Create(asyncActions.Select(x => x.Invoke()));

            return nextAsyncActionOption.TryGetValue(out var nextAsyncAction)
                ? Actions.Enumerator.Enumerator<TContext>.Create(enumeratingAsyncAction, nextAsyncAction)
                : enumeratingAsyncAction;
        }
    }
}