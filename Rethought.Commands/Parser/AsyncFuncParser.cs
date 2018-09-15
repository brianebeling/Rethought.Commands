using System;
using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace Rethought.Commands.Parser
{
    public sealed class AsyncFuncParser<TInput, TOutput> : IAsyncTypeParser<TInput, TOutput>
    {
        private readonly Func<TInput, CancellationToken, Task<Option<TOutput>>> func;

        private AsyncFuncParser(Func<TInput, CancellationToken, Task<Option<TOutput>>> func)
        {
            this.func = func;
        }

        public Task<Option<TOutput>> ParseAsync(TInput input, CancellationToken cancellationToken)
        {
            return this.func.Invoke(input, cancellationToken);
        }

        public static AsyncFuncParser<TInput, TOutput> Create(
            Func<TInput, CancellationToken, Task<Option<TOutput>>> func)
        {
            return new AsyncFuncParser<TInput, TOutput>(func);
        }

        public static AsyncFuncParser<TInput, TOutput> Create(
            Func<TInput, Task<Option<TOutput>>> func)
        {
            return new AsyncFuncParser<TInput, TOutput>((input, _) => func.Invoke(input));
        }
    }
}