using Optional;
using Rethought.Commands.Actions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Strategies
{
    public class AsyncAction<TContext> : IStrategy<TContext>
    {
        private readonly IAsyncAction<TContext> asyncAction;

        public AsyncAction(IAsyncAction<TContext> asyncAction)
        {
            this.asyncAction = asyncAction;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            return nextAsyncActionOption.TryGetValue(out var nextAsyncAction)
                ? Actions.Enumerating.Enumerator<TContext>.Create(asyncAction, nextAsyncAction)
                : asyncAction;
        }
    }
}