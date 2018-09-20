using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerating
{
    public sealed class PersistingAllOrFailedFactory<TContext> : IFactory<TContext>
    {
        public IAsyncAction<TContext> Create(IEnumerable<IAsyncAction<TContext>> asyncActions)
        {
            return PersistingAllOrFailed<TContext>.Create(asyncActions);
        }
    }
}