using System;
using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace Rethought.Commands.Parser
{
    public sealed class AsyncFunc<TInput, TOutput> : IAsyncTypeParser<TInput, TOutput>
    {
        private readonly Func<TInput, CancellationToken, Task<Option<TOutput, bool>>> func;

        private AsyncFunc(Func<TInput, CancellationToken, Task<Option<TOutput, bool>>> func)
        {
            this.func = func;
        }

        public Task<Option<TOutput, bool>> ParseAsync(TInput input, CancellationToken cancellationToken)
        {
            return func.Invoke(input, cancellationToken);
        }

        public static AsyncFunc<TInput, TOutput> Create(
            Func<TInput, CancellationToken, Task<Option<TOutput, bool>>> func)
        {
            return new AsyncFunc<TInput, TOutput>(func);
        }

        public static AsyncFunc<TInput, TOutput> Create(System.Func<TInput, Task<Option<TOutput, bool>>> func)
        {
            return new AsyncFunc<TInput, TOutput>((input, _) => func.Invoke(input));
        }
    }
}