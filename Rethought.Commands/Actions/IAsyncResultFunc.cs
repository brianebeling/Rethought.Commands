using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions
{
    public interface IAsyncResultFunc<in TContext>
    {
        Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken);
    }
}