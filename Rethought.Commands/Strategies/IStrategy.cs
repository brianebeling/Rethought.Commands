using Optional;
using Rethought.Commands.Actions;

namespace Rethought.Commands.Strategies
{
    public interface IStrategy<TContext>
    {
        IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption);
    }
}