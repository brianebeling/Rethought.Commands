using System;
using System.Collections.Generic;
using System.Linq;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Builder.Visitors;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder
{
    public class AsyncFuncBuilder<TContext>
    {
        protected internal readonly IList<Visitor<TContext>> Visitors = new List<Visitor<TContext>>();

        private AsyncFuncBuilder()
        {
        }

        private AsyncFuncBuilder(IEnumerable<Visitor<TContext>> visitors)
        {
            this.Visitors = visitors.ToList();
        }

        public AsyncFuncBuilder<TContext> AddStrategy(Visitor<TContext> buildingStep)
        {
            Visitors.Add(buildingStep);

            return this;
        }

        public AsyncFuncBuilder<TContext> InsertStrategy(Visitor<TContext> buildingStep, int index)
        {
            Visitors.Insert(index, buildingStep);

            return this;
        }

        public AsyncFuncBuilder<TContext> RemoveStrategy(Visitor<TContext> buildingStep)
        {
            Visitors.Remove(buildingStep);

            return this;
        }

        public AsyncFuncBuilder<TContext> RemoveStrategy(int index)
        {
            Visitors.RemoveAt(index);

            return this;
        }

        public IAsyncResultFunc<TContext> Build()
        {
            Option<IAsyncResultFunc<TContext>> next = default;

            foreach (var action in Visitors.Reverse())
            {
                next = action.Invoke(next).Some();
            }

            if (!next.TryGetValue(out var value)) throw new InvalidOperationException("Invalid configuration. There must at least be one visitor.");

            Visitors.Clear();

            return value;
        }

        public static AsyncFuncBuilder<TContext> Create()
        {
            return new AsyncFuncBuilder<TContext>();
        }

        public static AsyncFuncBuilder<TContext> Create(IEnumerable<Visitor<TContext>> visitors)
        {
            return new AsyncFuncBuilder<TContext>(visitors);
        }
    }
}