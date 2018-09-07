using System.Threading.Tasks;

namespace Rethought.Commands.Conditions
{
    public interface IAsyncCondition<in TContext>
    {
        Task<ConditionResult> SatisfiedAsync(TContext context);
    }
}