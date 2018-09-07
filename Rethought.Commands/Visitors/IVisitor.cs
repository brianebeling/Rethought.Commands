using Optional;
using Rethought.Commands.Actions;

namespace Rethought.Commands.Visitors
{
    public interface IVisitor<TContext>
    {
        IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption);
    }
}