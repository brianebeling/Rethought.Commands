using System;
using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace Rethought.Commands.Parser
{
    public sealed class AsyncAbortableFunc<TInput, TOutput> : IAsyncAbortableTypeParser<TInput, TOutput>
    {
        private readonly Func<TInput, CancellationToken, Task<(bool, Option<TOutput>)>> func;

        private AsyncAbortableFunc(Func<TInput, CancellationToken, Task<(bool, Option<TOutput>)>> func)
        {
            this.func = func;
        }


        public async Task<(bool Completed, Option<TOutput> Result)> ParseAsync(
            TInput input,
            CancellationToken cancellationToken)
        {
            return await func.Invoke(input, cancellationToken).ConfigureAwait(false);
        }


        public static AsyncAbortableFunc<TInput, TOutput> Create(
            Func<TInput, CancellationToken, Task<(bool, Option<TOutput>)>> func)
        {
            return new AsyncAbortableFunc<TInput, TOutput>(func);
        }

        public static AsyncAbortableFunc<TInput, TOutput> Create(
            System.Func<TInput, Task<(bool, Option<TOutput>)>> func)
        {
            return new AsyncAbortableFunc<TInput, TOutput>((input, _) => func.Invoke(input));
        }
    }
}