using System;
using Optional;
using Rethought.Commands.Actions;

namespace Rethought.Commands.Visitors
{
    public class FunctionalVisitor<TContext> : IVisitor<TContext>
    {
        private readonly Func<Option<IAsyncAction<TContext>>, IAsyncAction<TContext>> func;

        public FunctionalVisitor(Func<Option<IAsyncAction<TContext>>, IAsyncAction<TContext>> func)
        {
            this.func = func;
        }

        public IAsyncAction<TContext> Invoke(Option<IAsyncAction<TContext>> nextAsyncActionOption)
        {
            return func.Invoke(nextAsyncActionOption);
        }
    }
}