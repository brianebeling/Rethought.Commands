using Rethought.Commands.Actions;
using Rethought.Commands.Builder.Visitors;

namespace Rethought.Commands.Builder
{
    public interface IAsyncFuncBuilder<TContext>
    {
        AsyncFuncBuilder<TContext> AddStrategy(IStrategy<TContext> buildingStep);
        AsyncFuncBuilder<TContext> InsertStrategy(IStrategy<TContext> buildingStep, int index);
        AsyncFuncBuilder<TContext> RemoveStrategy(IStrategy<TContext> buildingStep);
        AsyncFuncBuilder<TContext> RemoveStrategy(int index);
        IAsyncResultFunc<TContext> Build();
    }
}