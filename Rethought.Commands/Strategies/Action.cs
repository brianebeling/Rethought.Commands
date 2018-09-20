using System.Threading;
using System.Threading.Tasks;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Adapter;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Strategies
{
    public class Action<TContext> : IStrategy<TContext>
    {
        private readonly IAction<TContext> action;

        public Action(IAction<TContext> action)
        {
            this.action = action;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            var asyncAction = action.ToAsyncBackground();

            async Task<bool> Func(TContext context, CancellationToken cancellationToken)
            {
                return await asyncAction.InvokeAsync(context, cancellationToken);
            }

            var asyncActionSystemAdapter = FuncAdapter<TContext>.Create(Func);

            if (nextAsyncActionOption.TryGetValue(out var nextAction))
            {
                return Actions.Enumerating.Enumerator<TContext>.Create(asyncActionSystemAdapter, nextAction);
            }

            return asyncActionSystemAdapter;
        }
    }
}