using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Enumerating
{
    public sealed class AnyOrFailed<TContext> : IAsyncAction<TContext>
    {
        private readonly IEnumerable<IAsyncAction<TContext>> actionsAsyncs;

        private AnyOrFailed(IEnumerable<IAsyncAction<TContext>> actionAsyncsAsyncs)
        {
            actionsAsyncs = actionAsyncsAsyncs;
        }

        public async Task<ActionResult> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            foreach (var actionAsync in actionsAsyncs)
            {
                var actionResult = await actionAsync.InvokeAsync(context, cancellationToken).ConfigureAwait(false);

                if (actionResult.State == State.Completed) return actionResult;
            }

            return ActionResult.Failed;
        }

        public static AnyOrFailed<TContext> Create(params IAsyncAction<TContext>[] actionAsyncs)
        {
            return new AnyOrFailed<TContext>(actionAsyncs);
        }

        public static AnyOrFailed<TContext> Create(
            IEnumerable<IAsyncAction<TContext>> actionAsyncs)
        {
            return new AnyOrFailed<TContext>(actionAsyncs);
        }
    }
}