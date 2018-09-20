using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Enumerating
{
    public sealed class AllOrFailed<TContext> : IAsyncAction<TContext>
    {
        private readonly IEnumerable<IAsyncAction<TContext>> actionsAsyncs;

        private AllOrFailed(IEnumerable<IAsyncAction<TContext>> actionAsyncsAsyncs)
        {
            actionsAsyncs = actionAsyncsAsyncs;
        }

        public async Task<bool> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            foreach (var actionAsync in actionsAsyncs)
            {
                var actionResult = await actionAsync.InvokeAsync(context, cancellationToken).ConfigureAwait(false);

                if (!actionResult) return false;
            }

            return true;
        }

        public static AllOrFailed<TContext> Create(params IAsyncAction<TContext>[] actionAsyncs)
        {
            return new AllOrFailed<TContext>(actionAsyncs);
        }

        public static AllOrFailed<TContext> Create(
            IEnumerable<IAsyncAction<TContext>> actionAsyncs)
        {
            return new AllOrFailed<TContext>(actionAsyncs);
        }
    }
}