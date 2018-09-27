using Optional;
using Rethought.Commands.Actions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class AsyncResultFunc<TContext> : IStrategy<TContext>
    {
        private readonly IAsyncResultFunc<TContext> asyncResultFunc;

        public AsyncResultFunc(IAsyncResultFunc<TContext> asyncResultFunc)
        {
            this.asyncResultFunc = asyncResultFunc;
        }

        public IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            return nextAsyncActionOption.TryGetValue(out var nextAsyncAction)
                ? Actions.Enumerator.Enumerator<TContext>.Create(asyncResultFunc, nextAsyncAction)
                : asyncResultFunc;
        }
    }
}