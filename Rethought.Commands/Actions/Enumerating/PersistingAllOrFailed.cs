using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Enumerating
{
    public sealed class PersistingAllOrFailed<TContext> : IAsyncAction<TContext>
    {
        private readonly IEnumerable<IAsyncAction<TContext>> actionsAsyncs;

        private PersistingAllOrFailed(IEnumerable<IAsyncAction<TContext>> actionAsyncsAsyncs)
        {
            actionsAsyncs = actionAsyncsAsyncs;
        }

        public async Task<bool> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            var finalActionResult = true;

            foreach (var actionAsync in actionsAsyncs)
            {
                var actionResult = await actionAsync.InvokeAsync(context, cancellationToken).ConfigureAwait(false);

                if (!actionResult)
                    finalActionResult = false;
            }

            return finalActionResult;
        }

        public static PersistingAllOrFailed<TContext> Create(
            params IAsyncAction<TContext>[] actionAsyncs)
        {
            return new PersistingAllOrFailed<TContext>(actionAsyncs);
        }

        public static PersistingAllOrFailed<TContext> Create(
            IEnumerable<IAsyncAction<TContext>> actionAsyncs)
        {
            return new PersistingAllOrFailed<TContext>(actionAsyncs);
        }
    }
}