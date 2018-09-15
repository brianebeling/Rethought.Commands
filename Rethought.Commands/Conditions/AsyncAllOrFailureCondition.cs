using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Rethought.Extensions.Optional;

namespace Rethought.Commands.Conditions
{
    public class AsyncAllOrFailureCondition<TContext> : IAsyncCondition<TContext>
    {
        private readonly IEnumerable<IAsyncCondition<TContext>> asyncConditions;

        public AsyncAllOrFailureCondition(IEnumerable<IAsyncCondition<TContext>> asyncConditions)
        {
            this.asyncConditions = asyncConditions;
        }

        public async Task<bool> SatisfiedAsync(TContext context)
        {
            foreach (var asyncCondition in asyncConditions)
            {
                if (!await asyncCondition.SatisfiedAsync(context).ConfigureAwait(false))
                {
                    return false;
                }
            }

            return true;
        }
    }
}