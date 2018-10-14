using System.Threading;
using System.Threading.Tasks;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Adapters.Action;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class Action<TContext> : IVisitor<TContext>
    {
        private readonly IAction<TContext> action;

        public Action(IAction<TContext> action)
        {
            this.action = action;
        }

        public IAsyncResultFunc<TContext> Invoke(Option<IAsyncResultFunc<TContext>> nextAsyncActionOption)
        {
            var asyncAction = action.ToAsyncBackgroundFunc();

            async Task<Result> Func(TContext context, CancellationToken cancellationToken)
            {
                await asyncAction.InvokeAsync(context, cancellationToken);
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