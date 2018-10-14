using Optional;
using Rethought.Commands.Actions;

namespace Rethought.Commands.Builder.Visitors
{
    public interface IVisitor<TContext>
    {
        IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption);
    }
}