using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerator
{
    public sealed class EnumeratorFactory<TContext> : IFactory<TContext>
    {
        public IAsyncResultFunc<TContext> Create(IEnumerable<IAsyncResultFunc<TContext>> asyncActions)
            => Enumerator<TContext>.Create(asyncActions);
    }
}