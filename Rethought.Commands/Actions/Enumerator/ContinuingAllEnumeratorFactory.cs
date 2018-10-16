using System;
using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerator
{
    public sealed class ContinuingAllEnumeratorFactory<TContext> : IEnumeratorFactory<TContext>
    {
        private readonly Func<Result, bool> predicate;

        public ContinuingAllEnumeratorFactory(Func<Result, bool> predicate)
        {
            this.predicate = predicate;
        }

        public IAsyncResultFunc<TContext> Create(IEnumerable<IAsyncResultFunc<TContext>> asyncActions)
            => ContinuingAllEnumerator<TContext>.Create(asyncActions, predicate);
    }
}