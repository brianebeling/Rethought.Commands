using Optional;
using Rethought.Commands.Action;

namespace Rethought.Commands.Builder.Visitors
{
    public interface IVisitor<TContext>
    {
        IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption);
    }
}