using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rethought.Commands.Conditions
{
    public class AsyncAllOrFailed<TContext> : IAsyncCondition<TContext>
    {
        private readonly IEnumerable<IAsyncCondition<TContext>> asyncConditions;

        public AsyncAllOrFailed(IEnumerable<IAsyncCondition<TContext>> asyncConditions)
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