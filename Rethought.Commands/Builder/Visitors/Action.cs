using System.Threading;
using System.Threading.Tasks;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Adapters.Action;
using Rethought.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class Action<TContext> : Visitor<TContext>
    {
        private readonly IAction<TContext> action;

        public Action(IAction<TContext> action)
        {
            this.action = action;
        }

        public override IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            var asyncAction = action.ToAsyncBackgroundFunc();

            async Task<Result> Func(TContext context, CancellationToken cancellationToken)
            {
                await asyncAction.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
                return Result.Completed;
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