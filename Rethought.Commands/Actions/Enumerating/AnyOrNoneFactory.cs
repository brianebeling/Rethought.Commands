using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerating
{
    public sealed class AnyOrNoneFactory<TContext> : IFactory<TContext>
    {
        public IAsyncAction<TContext> Create(IEnumerable<IAsyncAction<TContext>> asyncActions)
        {
            return AnyOrNone<TContext>.Create(asyncActions);
        }
    }
}