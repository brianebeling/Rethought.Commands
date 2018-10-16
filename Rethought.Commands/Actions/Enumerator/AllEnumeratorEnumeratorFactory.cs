using System;
using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerator
{
    public sealed class AllEnumeratorEnumeratorFactory<TContext> : IEnumeratorFactory<TContext>
    {
        private readonly Func<Result, bool> predicate;

        public AllEnumeratorEnumeratorFactory(Func<Result, bool> predicate)
        {
            this.predicate = predicate;
        }

        public IAsyncResultFunc<TContext> Create(IEnumerable<IAsyncResultFunc<TContext>> asyncActions)
            => AllEnumerator<TContext>.Create(asyncActions, predicate);
    }
}