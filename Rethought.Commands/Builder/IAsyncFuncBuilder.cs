using Rethought.Commands.Actions;
using Rethought.Commands.Builder.Visitors;

namespace Rethought.Commands.Builder
{
    public interface IAsyncFuncBuilder<TContext>
    {
        AsyncFuncBuilder<TContext> AddStrategy(IVisitor<TContext> buildingStep);
        AsyncFuncBuilder<TContext> InsertStrategy(IVisitor<TContext> buildingStep, int index);
        AsyncFuncBuilder<TContext> RemoveStrategy(IVisitor<TContext> buildingStep);
        AsyncFuncBuilder<TContext> RemoveStrategy(int index);
        IAsyncResultFunc<TContext> Build();
    }
}