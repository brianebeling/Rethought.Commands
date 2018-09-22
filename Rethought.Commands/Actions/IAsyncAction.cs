using System.Threading;
using System.Threading.Tasks;

namespace Rethought.Commands.Actions
{
    public interface IAsyncAction<in TContext>
    {
        Task<Result> InvokeAsync(TContext context, CancellationToken cancellationToken);
    }

    public enum Result
    {
        None,
        Completed,
        Aborted
    }
}