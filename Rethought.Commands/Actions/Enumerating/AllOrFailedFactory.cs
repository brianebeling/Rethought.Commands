using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerating
{
    public sealed class AllOrFailedFactory<TContext> : IFactory<TContext>
    {
        public IAsyncAction<TContext> Create(IEnumerable<IAsyncAction<TContext>> asyncActions)
        {
            return AllOrFailed<TContext>.Create(asyncActions);
        }
    }
}