using System;
using System.Collections.Generic;
using Optional;
using Rethought.Commands.Actions;
using Rethought.Commands.Visitors;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Builder
{
    public class AsyncActionBuilder<TContext>
    {
        protected readonly IList<IVisitor<TContext>> BuildingSteps = new List<IVisitor<TContext>>();

        public AsyncActionBuilder<TContext> AddBuildingStep(IVisitor<TContext> buildingStep)
        {
            BuildingSteps.Add(buildingStep);

            return this;
        }

        /// <summary>
        ///     Sets a prototype. A prototype is inserted as the first visitor.
        ///     Use this if you want to extend an existing <see cref="IAsyncAction{TContext}" />.
        /// </summary>
        /// <param name="asyncAction">The action asynchronous.</param>
        /// <returns></returns>
        public AsyncActionBuilder<TContext> WithPrototype(IAsyncAction<TContext> asyncAction)
        {
            BuildingSteps.Insert(0, new PrototypeVisitor<TContext>(asyncAction));

            return this;
        }

        public virtual IAsyncAction<TContext> Build()
        {
            Option<IAsyncAction<TContext>> next = default;

            foreach (var action in BuildingSteps)
            {
                next = action.Invoke(next).Some();
            }

            if (!next.TryGetValue(out var value)) throw new InvalidOperationException("The configuration is invalid");

            BuildingSteps.Clear();

            return value;
        }
    }
}