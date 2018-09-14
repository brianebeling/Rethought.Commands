using System;
using System.Collections.Generic;
using System.Linq;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Strategies;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder
{
    public class AsyncActionBuilder<TContext>
    {
        protected readonly IList<IStrategy<TContext>> Strategies = new List<IStrategy<TContext>>();

        public AsyncActionBuilder<TContext> AddStrategy(IStrategy<TContext> buildingStep)
        {
            Strategies.Add(buildingStep);

            return this;
        }

        /// <summary>
        ///     Sets a prototype. A prototype is inserted as the first strategy.
        ///     Use this if you want to extend an existing <see cref="IAsyncAction{TContext}" />.
        /// </summary>
        /// <param name="asyncAction">The action asynchronous.</param>
        /// <returns></returns>
        public AsyncActionBuilder<TContext> WithPrototype(IAsyncAction<TContext> asyncAction)
        {
            Strategies.Insert(0, new PrototypeStrategy<TContext>(asyncAction));

            return this;
        }

        public virtual IAsyncAction<TContext> Build()
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