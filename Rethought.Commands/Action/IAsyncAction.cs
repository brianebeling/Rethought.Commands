using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Action
{
    public interface IAsyncAction<in TContext>
    {
        Task InvokeAsync(TContext context, CancellationToken cancellationToken);
    }
}