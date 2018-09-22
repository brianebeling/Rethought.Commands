using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerating
{
    public sealed class PersistingAllOrAbortedFactory<TContext> : IFactory<TContext>
    {
        public IAsyncAction<TContext> Create(IEnumerable<IAsyncAction<TContext>> asyncActions)
        {
            return PersistingAllOrAborted<TContext>.Create(asyncActions);
        }
    }
}