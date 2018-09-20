using System;
using System.Collections.Generic;
using System.Linq;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Strategies;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder
{
    public class AsyncActionBuilder<TContext> : IAsyncActionBuilder<TContext>
    {
        protected internal readonly IList<IStrategy<TContext>> Strategies = new List<IStrategy<TContext>>();

        public AsyncActionBuilder<TContext> AddStrategy(IStrategy<TContext> buildingStep)
        {
            Strategies.Add(buildingStep);

            return this;
        }

        public AsyncActionBuilder<TContext> InsertStrategy(IStrategy<TContext> buildingStep, int index)
        {
            Strategies.Insert(index, buildingStep);

            return this;
        }

        public AsyncActionBuilder<TContext> RemoveStrategy(IStrategy<TContext> buildingStep)
        {
            Strategies.Remove(buildingStep);

            return this;
        }

        public AsyncActionBuilder<TContext> RemoveStrategy(int index)
        {
            Strategies.RemoveAt(index);

            return this;
        }

        public IAsyncAction<TContext> Build()
        {
            Option<IAsyncAction<TContext>> next = default;

            foreach (var action in Strategies.Reverse())
            {
                next = action.Invoke(next).Some();
            }

            if (!next.TryGetValue(out var value)) throw new InvalidOperationException("The configuration is invalid");

            Strategies.Clear();

            return value;
        }
    }
}