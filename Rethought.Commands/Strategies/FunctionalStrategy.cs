using System;
using Optional;
using Rethought.Commands.Actions;

namespace Rethought.Commands.Visitors
{
    public class FunctionalStrategy<TContext> : IStrategy<TContext>
    {
        private readonly Func<Option<IAsyncAction<TContext>>, IAsyncAction<TContext>> func;

        public FunctionalStrategy(Func<Option<IAsyncAction<TContext>>, IAsyncAction<TContext>> func)
        {
            this.func = func;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            return func.Invoke(nextAsyncActionOption);
        }
    }
}