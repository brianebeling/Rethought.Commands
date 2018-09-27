using System;
using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerator
{
    public sealed class ContinuingAllFactory<TContext> : IFactory<TContext>
    {
        private readonly Func<Result, bool> predicate;

        public ContinuingAllFactory(Func<Result, bool> predicate)
        {
            this.predicate = predicate;
        }

        public IAsyncResultFunc<TContext> Create(IEnumerable<IAsyncResultFunc<TContext>> asyncActions)
        {
            return ContinuingAll<TContext>.Create(asyncActions, predicate);
        }
    }
}