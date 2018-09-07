using Optional;
using Rethought.Commands.Actions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Visitors
{
    public class AsyncActionVisitor<TContext> : IVisitor<TContext>
    {
        private readonly IAsyncAction<TContext> asyncAction;

        public AsyncActionVisitor(IAsyncAction<TContext> asyncAction)
        {
            this.asyncAction = asyncAction;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            return nextAsyncActionOption.TryGetValue(out var nextAsyncAction)
                ? EnumeratingAsyncAction<TContext>.Create(asyncAction, nextAsyncAction)
                : asyncAction;
        }
    }
}