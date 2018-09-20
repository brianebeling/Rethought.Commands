using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Enumerating
{
    public sealed class Enumerator<TContext> : IAsyncAction<TContext>
    {
        private readonly IEnumerable<IAsyncAction<TContext>> actionsAsyncs;

        private Enumerator(IEnumerable<IAsyncAction<TContext>> actionAsyncsAsyncs)
        {
            actionsAsyncs = actionAsyncsAsyncs;
        }

        public async Task<bool> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            foreach (var actionAsync in actionsAsyncs)
            {
                await actionAsync.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
            }

            return true;
        }

        public static Enumerator<TContext> Create(params IAsyncAction<TContext>[] actionAsyncs)
        {
            return new Enumerator<TContext>(actionAsyncs);
        }

        public static Enumerator<TContext> Create(IEnumerable<IAsyncAction<TContext>> actionAsyncs)
        {
            return new Enumerator<TContext>(actionAsyncs);
        }
    }
}