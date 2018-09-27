using System;
using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerator
{
    public sealed class AllFactory<TContext> : IFactory<TContext>
    {
        private readonly Func<Result, bool> predicate;

        public AllFactory(Func<Result, bool> predicate)
        {
            this.predicate = predicate;
        }

        public IAsyncResultFunc<TContext> Create(IEnumerable<IAsyncResultFunc<TContext>> asyncActions)
        {
            return All<TContext>.Create(asyncActions, predicate);
        }
    }
}