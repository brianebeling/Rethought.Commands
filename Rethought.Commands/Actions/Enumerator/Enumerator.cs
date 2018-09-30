using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Enumerator
{
    public sealed class Enumerator<TContext> : IAsyncResultFunc<TContext>
    {
        private readonly IEnumerable<IAsyncResultFunc<TContext>> actionsAsyncs;

        private Enumerator(IEnumerable<IAsyncResultFunc<TContext>> actionAsyncsAsyncs)
        {
            actionsAsyncs = actionAsyncsAsyncs;
        }

        public async Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            foreach (var actionAsync in actionsAsyncs)
            {
                await actionAsync.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
            }

            return Result.Completed;
        }

        public static Enumerator<TContext> Create(params IAsyncResultFunc<TContext>[] resultFuncAsyncsResult)
            => new Enumerator<TContext>(resultFuncAsyncsResult);

        public static Enumerator<TContext> Create(IEnumerable<IAsyncResultFunc<TContext>> actionAsyncs)
            => new Enumerator<TContext>(actionAsyncs);
    }
}