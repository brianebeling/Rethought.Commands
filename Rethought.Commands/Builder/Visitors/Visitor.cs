using Rethought.Commands.Actions;
using Rethought.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public abstract class Visitor<TContext>
    {
        public abstract IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption);
    }
}