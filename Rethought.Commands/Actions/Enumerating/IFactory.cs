using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerating
{
    public interface IFactory<TContext>
    {
        IAsyncAction<TContext> Create(IEnumerable<IAsyncAction<TContext>> asyncActions);
    }
}