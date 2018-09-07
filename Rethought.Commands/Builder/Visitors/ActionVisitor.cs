using System.Threading;
using System.Threading.Tasks;
using Optional;
using Rethought.Commands.Action;
using Rethought.Commands.Action.Adapter;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder.Visitors
{
    public class ActionVisitor<TContext> : IVisitor<TContext>
    {
        private readonly IAction<TContext> asyncAction;

        public ActionVisitor(IAction<TContext> asyncAction)
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
                return EnumeratingAsyncAction<TContext>.Create(asyncActionSystemAdapter,
                    nextAction);
            }

            return asyncActionSystemAdapter;
        }
    }
}