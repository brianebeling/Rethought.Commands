using System;
using System.Collections.Generic;
using System.Linq;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Builder.Visitors;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder
{
    public class AsyncFuncBuilder<TContext> : IAsyncFuncBuilder<TContext>
    {
        protected internal readonly IList<IStrategy<TContext>> Strategies = new List<IStrategy<TContext>>();

        private AsyncFuncBuilder()
        {
        }

        public AsyncFuncBuilder<TContext> AddStrategy(IStrategy<TContext> buildingStep)
        {
            Strategies.Add(buildingStep);

            return this;
        }

        public AsyncFuncBuilder<TContext> InsertStrategy(IStrategy<TContext> buildingStep, int index)
        {
            Strategies.Insert(index, buildingStep);

            return this;
        }

        public AsyncFuncBuilder<TContext> RemoveStrategy(IStrategy<TContext> buildingStep)
        {
            Strategies.Remove(buildingStep);

            return this;
        }

        public AsyncFuncBuilder<TContext> RemoveStrategy(int index)
        {
            Strategies.RemoveAt(index);

            return this;
        }

        public IAsyncResultFunc<TContext> Build()
        {
            Option<IAsyncResultFunc<TContext>> next = default;

            foreach (var action in Strategies.Reverse())
            {
                next = action.Invoke(next).Some();
            }

            if (!next.TryGetValue(out var value)) throw new InvalidOperationException("The configuration is invalid");

            Strategies.Clear();

            return value;
        }

        public static AsyncFuncBuilder<TContext> Create()
        {
            return new AsyncFuncBuilder<TContext>();
        }
    }
}