using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerator
{
    public interface IEnumeratorFactory<TContext>
    {
        IAsyncResultFunc<TContext> Create(IEnumerable<IAsyncResultFunc<TContext>> asyncActions);
    }
}