using System;

namespace Rethought.Commands.Actions.Adapter
{
    public sealed class FuncAdapter<TContext> : IAction<TContext>
    {
        private readonly Func<TContext, Result> func;

        private FuncAdapter(Func<TContext, Result> func)
        {
            this.func = func;
        }

        public Result Invoke(TContext context)
        {
            return func.Invoke(context);
        }

        public static FuncAdapter<TContext> Create(Func<TContext, Result> func)
        {
            return new FuncAdapter<TContext>(func);
        }
    }
}