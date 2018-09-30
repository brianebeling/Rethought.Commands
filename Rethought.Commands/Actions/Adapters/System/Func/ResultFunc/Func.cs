using System;

namespace Rethought.Commands.Actions.Adapters.System.Func.ResultFunc
{
    public sealed class Func<TContext> : IResultFunc<TContext>
    {
        private readonly Func<TContext, Result> func;

        private Func(Func<TContext, Result> func)
        {
            this.func = func;
        }

        public Result Invoke(TContext context)
            => func.Invoke(context);

        public static Func<TContext> Create(Func<TContext, Result> func)
            => new Func<TContext>(func);
    }
}