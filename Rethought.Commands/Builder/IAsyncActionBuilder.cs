using Rethought.Commands.Actions;
using Rethought.Commands.Strategies;

namespace Rethought.Commands.Builder
{
    public interface IAsyncActionBuilder<TContext>
    {
        AsyncActionBuilder<TContext> AddStrategy(IStrategy<TContext> buildingStep);
        AsyncActionBuilder<TContext> InsertStrategy(IStrategy<TContext> buildingStep, int index);
        AsyncActionBuilder<TContext> RemoveStrategy(IStrategy<TContext> buildingStep);
        AsyncActionBuilder<TContext> RemoveStrategy(int index);
        IAsyncAction<TContext> Build();
    }
}