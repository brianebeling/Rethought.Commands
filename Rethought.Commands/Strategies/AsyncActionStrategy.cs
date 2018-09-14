using Optional;
using Rethought.Commands.Actions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Strategies
{
    public class AsyncActionStrategy<TContext> : IStrategy<TContext>
    {
        private readonly IAsyncAction<TContext> asyncAction;

        public AsyncActionStrategy(IAsyncAction<TContext> asyncAction)
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