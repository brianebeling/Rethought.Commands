using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerating
{
    public sealed class EnumeratorFactory<TContext> : IFactory<TContext>
    {
        public IAsyncAction<TContext> Create(IEnumerable<IAsyncAction<TContext>> asyncActions)
        {
            return Enumerator<TContext>.Create(asyncActions);
        }
    }
}