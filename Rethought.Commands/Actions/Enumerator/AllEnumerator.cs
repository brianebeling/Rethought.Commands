﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions.Enumerator
{
    /// <summary>
    ///     This enumerator enumerates the collection and stops at the first <see cref="IAsyncResultFunc{TContext}" /> that
    ///     returns <see cref="Result.Aborted" />.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="IAsyncResultFunc{TContext}" />
    public sealed class AllEnumerator<TContext> : IAsyncResultFunc<TContext>
    {
        private readonly IEnumerable<IAsyncResultFunc<TContext>> actionsAsyncs;
        private readonly Func<Result, bool> predicate;

        private AllEnumerator(IEnumerable<IAsyncResultFunc<TContext>> actionAsyncsAsyncs, Func<Result, bool> predicate)
        {
            actionsAsyncs = actionAsyncsAsyncs;
            this.predicate = predicate;
        }

        public async Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken)
        {
            foreach (var actionAsync in actionsAsyncs)
            {
                var actionResult = await actionAsync.InvokeAsync(context, cancellationToken).ConfigureAwait(false);

                if (predicate.Invoke(actionResult)) return actionResult;
            }

            return Result.Completed;
        }

        public static AllEnumerator<TContext> Create(
            IEnumerable<IAsyncResultFunc<TContext>> actionAsyncs,
            Func<Result, bool> predicate)
            => new AllEnumerator<TContext>(actionAsyncs, predicate);
    }
}