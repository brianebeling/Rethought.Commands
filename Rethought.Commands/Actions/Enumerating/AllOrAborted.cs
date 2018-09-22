using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Enumerating
{
    public sealed class AllOrAborted<TContext> : IAsyncAction<TContext>
    {
        private readonly IEnumerable<IAsyncAction<TContext>> actionsAsyncs;

        private AllOrAborted(IEnumerable<IAsyncAction<TContext>> actionAsyncsAsyncs)
        {
            actionsAsyncs = actionAsyncsAsyncs;
        }

        public async Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            foreach (var actionAsync in actionsAsyncs)
            {
                var actionResult = await actionAsync.InvokeAsync(context, cancellationToken).ConfigureAwait(false);

                if (actionResult == Result.Aborted) return actionResult;
            }

            return Result.Completed;
        }

        public static AllOrAborted<TContext> Create(params IAsyncAction<TContext>[] actionAsyncs)
        {
            return new AllOrAborted<TContext>(actionAsyncs);
        }

        public static AllOrAborted<TContext> Create(
            IEnumerable<IAsyncAction<TContext>> actionAsyncs)
        {
            return new AllOrAborted<TContext>(actionAsyncs);
        }
    }
}