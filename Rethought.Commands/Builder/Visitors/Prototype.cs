using Optional;
using Rethought.Commands.Actions;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class Prototype<TContext> : Visitor<TContext>
    {
        private readonly IAsyncResultFunc<TContext> asyncResultFunc;

        public Prototype(IAsyncResultFunc<TContext> asyncResultFunc)
        {
            this.asyncResultFunc = asyncResultFunc;
        }

        public override IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
            => nextAsyncActionOption.TryGetValue(out var nextAction)
                ? Actions.Enumerator.Enumerator<TContext>.Create(asyncResultFunc, nextAction)
                : asyncResultFunc;
    }
}