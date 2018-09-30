using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace Rethought.Commands.Parser
{
    public interface ITypeParser<in TInput, TOutput>
    {
        Option<TOutput> Parse(TInput input);
    }

    public interface IAsyncTypeParser<in TInput, TOutput>
    {
        Task<Option<TOutput>> ParseAsync(TInput input, CancellationToken cancellationToken);
    }
}
