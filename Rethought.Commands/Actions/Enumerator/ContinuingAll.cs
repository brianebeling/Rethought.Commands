using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Enumerator
{
    /// <summary>
    ///     This enumerator enumerates the collection but does not stop at the first valid item
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public sealed class ContinuingAll<TContext> : IAsyncResultFunc<TContext>
    {
        private readonly IEnumerable<IAsyncResultFunc<TContext>> actionsAsyncs;
        private readonly Func<Result, bool> predicate;

        private ContinuingAll(IEnumerable<IAsyncResultFunc<TContext>> actionAsyncsAsyncs, Func<Result, bool> predicate)
        {
            actionsAsyncs = actionAsyncsAsyncs;
            this.predicate = predicate;
        }

        public async Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            var finalActionResult = Result.Completed;

            foreach (var actionAsync in actionsAsyncs)
            {
                var actionResult = await actionAsync.InvokeAsync(context, cancellationToken).ConfigureAwait(false);

                if (predicate.Invoke(actionResult))
                    finalActionResult = actionResult;
            }

            return finalActionResult;
        }

        public static ContinuingAll<TContext> Create(
            IEnumerable<IAsyncResultFunc<TContext>> actionAsyncs,
            Func<Result, bool> predicate)
        {
            return new ContinuingAll<TContext>(actionAsyncs, predicate);
        }
    }
}