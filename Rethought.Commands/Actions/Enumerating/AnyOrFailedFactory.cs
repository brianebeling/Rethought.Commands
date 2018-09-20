using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerating
{
    public sealed class AnyOrFailedFactory<TContext> : IFactory<TContext>
    {
        public IAsyncAction<TContext> Create(IEnumerable<IAsyncAction<TContext>> asyncActions)
        {
            return AnyOrFailed<TContext>.Create(asyncActions);
        }
    }
}