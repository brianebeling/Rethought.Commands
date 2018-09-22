using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerating
{
    public sealed class AllOrAbortedFactory<TContext> : IFactory<TContext>
    {
        public IAsyncAction<TContext> Create(IEnumerable<IAsyncAction<TContext>> asyncActions)
        {
            return AllOrAborted<TContext>.Create(asyncActions);
        }
    }
}