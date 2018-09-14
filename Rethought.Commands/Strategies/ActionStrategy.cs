using System.Threading;
using System.Threading.Tasks;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Actions.Adapter;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Visitors
{
    public class ActionStrategy<TContext> : IStrategy<TContext>
    {
        private readonly IAction<TContext> asyncAction;

        public ActionStrategy(IAction<TContext> asyncAction)
        {
            this.asyncAction = asyncAction;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            Task Func(TContext context, CancellationToken _)
            {
                asyncAction.Invoke(context);
                return Task.CompletedTask;
            }

            var asyncActionSystemAdapter = FuncAdapter<TContext>.Create(Func);  

            if (nextAsyncActionOption.TryGetValue(out var nextAction))
            {
                return EnumeratingAsyncAction<TContext>.Create(asyncActionSystemAdapter, nextAction);
            }

            return asyncActionSystemAdapter;
        }
    }
}