using System.Threading;
using System.Threading.Tasks;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Adapters.ResultFunc;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class Func<TContext> : IVisitor<TContext>
    {
        private readonly IResultFunc<TContext> resultFunc;

        public Func(IResultFunc<TContext> resultFunc)
        {
            this.resultFunc = resultFunc;
        }

        public IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            var asyncAction = resultFunc.ToAsyncBackgroundResultFunc();

            async Task<Result> Func(TContext context, CancellationToken cancellationToken)
            {
                return await asyncAction.InvokeAsync(context, cancellationToken);
            }

            var asyncActionSystemAdapter = Actions.Adapters.System.Func.AsyncResultFunc.Func<TContext>.Create(Func);

            if (nextAsyncActionOption.TryGetValue(out var nextAction))
            {
                return Actions.Enumerator.Enumerator<TContext>.Create(asyncActionSystemAdapter, nextAction);
            }

            return asyncActionSystemAdapter;
        }
    }
}