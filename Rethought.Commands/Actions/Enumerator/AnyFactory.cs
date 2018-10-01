using System;
using System.Collections.Generic;

namespace Rethought.Commands.Actions.Enumerator
{
    public sealed class AnyFactory<TContext> : IFactory<TContext>
    {
        private readonly Func<Result, bool> predicate;

        public AnyFactory(Func<Result, bool> predicate)
        {
            this.predicate = predicate;
        }

        public IAsyncResultFunc<TContext> Create(IEnumerable<IAsyncResultFunc<TContext>> asyncActions)
            => Any<TContext>.Create(asyncActions, predicate);
    }
}