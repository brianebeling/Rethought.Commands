using System;
using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerator
{
    public sealed class AnyEnumeratorFactory<TContext> : IEnumeratorFactory<TContext>
    {
        private readonly Func<Result, bool> predicate;

        public AnyEnumeratorFactory(Func<Result, bool> predicate)
        {
            this.predicate = predicate;
        }

        public IAsyncResultFunc<TContext> Create(IEnumerable<IAsyncResultFunc<TContext>> asyncActions)
            => AnyEnumerator<TContext>.Create(asyncActions, predicate);
    }
}