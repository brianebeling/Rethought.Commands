using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions
{
    public interface IAsyncFunc<in TContext>
    {
        Task InvokeAsync(TContext context, CancellationToken cancellationToken);
    }
}