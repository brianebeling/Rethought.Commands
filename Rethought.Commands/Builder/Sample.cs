using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Optional;
using Rethought.Commands.Actions.Adapter;
using Rethought.Commands.Conditions;
using Rethought.Commands.Parser;

namespace Rethought.Commands.Builder
{
    public class Sample
    {
        public async Task Sample_1()
        {
            var asyncActionBuilder = new AsyncActionBuilder<DiscordContext>();

            var asyncAction =
                asyncActionBuilder
                    .WithConditions(
                        new List<ICondition<DiscordContext>>
                        {
                            new SenderMustEqualCondition("John Doe")
                        })
                    .WithCondition(new SenderMustEqualCondition("John Doe"))
                    .WithAsyncCondition(
                        new AsyncFuncCondition<DiscordContext>(_ => Task.FromResult(ConditionResult.Success)))
                    .WithAdapter(
                        new NaturalLanguageProcessingContextParser(),
                        adapterAsyncActionBuilder => adapterAsyncActionBuilder
                            .WithCondition(new SenderMustEqualCondition("Peter Griffin"))
                            .WithAsyncAction(FuncAdapter<NaturalLanguageContext>.Create((context, cancellationToken) =>
                            {
                                // Do something async since this is an asyncAction
                                Console.WriteLine(context.Intent);
                                return Task.CompletedTask;
                            }))
                    )
                    .Build();

            await asyncAction.InvokeAsync(
                new DiscordContext
                {
                    Sender = "John Doe",
                    Message = "I like big melons",
                    TimeStamp = new DateTimeOffset(DateTime.Now, -TimeSpan.FromDays(5))
                },
                CancellationToken.None);
        }

        /// <summary>
        ///     Parses the DiscordContext into a NaturalLanguageContext
        /// </summary>
        /// <seealso
        ///     cref="Parser.IAsyncTypeParser{DiscordContext, NaturalLanguageContext}" />
        public class NaturalLanguageProcessingContextParser : IAsyncTypeParser<DiscordContext, NaturalLanguageContext>
        {
            public Task<Option<NaturalLanguageContext>> ParseAsync(DiscordContext input,
                CancellationToken cancellationToken)
            {
                // somehow convert incoming context into the new one
                return Task.FromResult(Option.Some(new NaturalLanguageContext()));
            }
        }


        /// <summary>
        ///     The Discord Context (think Discord.Net.Commands)
        /// </summary>
        public class DiscordContext
        {
            public string Message { get; set; }
            public string Sender { get; set; }
            public DateTimeOffset TimeStamp { get; set; }
        }

        /// <summary>
        ///     The Discord Context, but with added information from NLP
        /// </summary>
        /// <seealso cref="DiscordContext" />
        public class NaturalLanguageContext : DiscordContext
        {
            public string Intent { get; set; }
        }

        /// <summary>
        ///     A condition that compares whether the senders name equals a specified string
        /// </summary>
        /// <seealso cref="Conditions.ICondition{DiscordContext}" />
        public class SenderMustEqualCondition : ICondition<DiscordContext>
        {
            private readonly string sender;

            public SenderMustEqualCondition(string sender)
            {
                this.sender = sender;
            }

            public ConditionResult Satisfied(DiscordContext context)
            {
                return context.Sender == sender
                    ? ConditionResult.Success
                    : ConditionResult.FailWithReason(new Reason("The Message sent is not from the sender specified"));
            }
        }
    }
}