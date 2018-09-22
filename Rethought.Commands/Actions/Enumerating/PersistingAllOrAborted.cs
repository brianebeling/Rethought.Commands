using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Enumerating
{
    public sealed class PersistingAllOrAborted<TContext> : IAsyncAction<TContext>
    {
        private readonly IEnumerable<IAsyncAction<TContext>> actionsAsyncs;

        private PersistingAllOrAborted(IEnumerable<IAsyncAction<TContext>> actionAsyncsAsyncs)
        {
            actionsAsyncs = actionAsyncsAsyncs;
        }

        public async Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            var finalActionResult = Result.Completed;

            foreach (var actionAsync in actionsAsyncs)
            {
                var actionResult = await actionAsync.InvokeAsync(context, cancellationToken).ConfigureAwait(false);

                if (actionResult == Result.Aborted)
                    finalActionResult = actionResult;
            }

            return finalActionResult;
        }

        public static PersistingAllOrAborted<TContext> Create(
            params IAsyncAction<TContext>[] actionAsyncs)
        {
            return new PersistingAllOrAborted<TContext>(actionAsyncs);
        }

        public static PersistingAllOrAborted<TContext> Create(
            IEnumerable<IAsyncAction<TContext>> actionAsyncs)
        {
            return new PersistingAllOrAborted<TContext>(actionAsyncs);
        }
    }
}