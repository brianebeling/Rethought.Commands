using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Enumerating
{
    public sealed class AnyOrNone<TContext> : IAsyncAction<TContext>
    {
        private readonly IEnumerable<IAsyncAction<TContext>> actionsAsyncs;

        private AnyOrNone(IEnumerable<IAsyncAction<TContext>> actionAsyncsAsyncs)
        {
            actionsAsyncs = actionAsyncsAsyncs;
        }

        public async Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            foreach (var actionAsync in actionsAsyncs)
            {
                var actionResult = await actionAsync.InvokeAsync(context, cancellationToken).ConfigureAwait(false);

                if (actionResult == Result.Completed || actionResult == Result.Aborted) return actionResult;
            }

            return Result.None;
        }

        public static AnyOrNone<TContext> Create(params IAsyncAction<TContext>[] actionAsyncs)
        {
            return new AnyOrNone<TContext>(actionAsyncs);
        }

        public static AnyOrNone<TContext> Create(
            IEnumerable<IAsyncAction<TContext>> actionAsyncs)
        {
            return new AnyOrNone<TContext>(actionAsyncs);
        }
    }
}