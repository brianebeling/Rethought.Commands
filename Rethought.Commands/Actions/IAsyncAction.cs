using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions
{
    public interface IAsyncAction<in TContext>
    {
        Task<bool> InvokeAsync(TContext context, CancellationToken cancellationToken);
    }
}