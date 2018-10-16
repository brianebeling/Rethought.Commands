using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerator
{
    public sealed class EnumeratorEnumeratorFactory<TContext> : IEnumeratorFactory<TContext>
    {
        public IAsyncResultFunc<TContext> Create(IEnumerable<IAsyncResultFunc<TContext>> asyncActions)
            => Enumerator<TContext>.Create(asyncActions);
    }
}