using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions
{
    public class EnumeratingAsyncAction<TContext> : IAsyncAction<TContext>
    {
        private readonly IEnumerable<IAsyncAction<TContext>> actionsAsyncs;

        public EnumeratingAsyncAction(IEnumerable<IAsyncAction<TContext>> actionAsyncsAsyncs)
        {
            actionsAsyncs = actionAsyncsAsyncs;
        }

        public async Task InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            foreach (var actionAsync in actionsAsyncs)
            {
                await actionAsync.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
            }
        }

        public static EnumeratingAsyncAction<TContext> Create(params IAsyncAction<TContext>[] actionAsyncs)
        {
            return new EnumeratingAsyncAction<TContext>(actionAsyncs);
        }
    }
}