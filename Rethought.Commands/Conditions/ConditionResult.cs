using System.Collections.Generic;
using System.Collections.ObjectModel;
using Optional;

namespace Rethought.Commands.Conditions
{
    public struct ConditionResult
    {
        public Option<IReadOnlyCollection<Reason>> Reason { get; set; }

        public bool Satisfied { get; set; }

        public static ConditionResult Fail = new ConditionResult {Satisfied = false};

        public static ConditionResult FailWithReason(Reason reason)
        {
            return new ConditionResult
            {
                Reason = Option.Some<IReadOnlyCollection<Reason>>(
                    new ReadOnlyCollection<Reason>(new List<Reason> {reason})),
                Satisfied = false
            };
        }

        public static ConditionResult FailWithReasons(IReadOnlyCollection<Reason> reason)
        {
            return new ConditionResult {Reason = Option.Some(reason), Satisfied = false};
        }

        public static ConditionResult SucceedWithReason(Reason reason)
        {
            return new ConditionResult
            {
                Reason = Option.Some<IReadOnlyCollection<Reason>>(
                    new ReadOnlyCollection<Reason>(new List<Reason> {reason})),
                Satisfied = true
            };
        }


        public static ConditionResult Success = new ConditionResult {Satisfied = true};
    }
}