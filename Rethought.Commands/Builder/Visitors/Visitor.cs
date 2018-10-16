using Optional;
using Rethought.Commands.Actions;

namespace Rethought.Commands.Builder.Visitors
{
    public abstract class Visitor<TContext>
    {
        public abstract IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption);
    }
}