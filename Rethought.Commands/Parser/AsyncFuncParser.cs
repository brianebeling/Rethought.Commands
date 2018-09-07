using System;
using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace Rethought.Commands.Parser
{
    public class AsyncFuncParser<TInput, TOutput> : IAsyncTypeParser<TInput, TOutput>
    {
        private readonly Func<TInput, CancellationToken, Task<Option<TOutput>>> func;

        public AsyncFuncParser(Func<TInput, CancellationToken, Task<Option<TOutput>>> func)
        {
            this.func = func;
        }

        public Task<Option<TOutput>> ParseAsync(TInput input, CancellationToken cancellationToken)
        {
            return this.func.Invoke(input, cancellationToken);
        }
    }
}