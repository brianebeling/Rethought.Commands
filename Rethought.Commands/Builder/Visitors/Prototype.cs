using Optional;
using Rethought.Commands.Actions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class Prototype<TContext> : IStrategy<TContext>
    {
        private readonly IAsyncResultFunc<TContext> asyncResultFunc;

        public Prototype(IAsyncResultFunc<TContext> asyncResultFunc)
        {
            this.asyncResultFunc = asyncResultFunc;
        }

        public IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            return nextAsyncActionOption.TryGetValue(out var nextAction)
                ? Actions.Enumerator.Enumerator<TContext>.Create(asyncResultFunc, nextAction)
                : asyncResultFunc;
        }
    }
}