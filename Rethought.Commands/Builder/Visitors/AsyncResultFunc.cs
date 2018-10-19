using Rethought.Commands.Actions;
using Rethought.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class AsyncResultFunc<TContext> : Visitor<TContext>
    {
        private readonly IAsyncResultFunc<TContext> asyncResultFunc;

        public AsyncResultFunc(IAsyncResultFunc<TContext> asyncResultFunc)
        {
            this.asyncResultFunc = asyncResultFunc;
        }

        public override IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
            => nextAsyncActionOption.TryGetValue(out var nextAsyncAction)
                ? Actions.Enumerator.Enumerator<TContext>.Create(asyncResultFunc, nextAsyncAction)
                : asyncResultFunc;
    }
}