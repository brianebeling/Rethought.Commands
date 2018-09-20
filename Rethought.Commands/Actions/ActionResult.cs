namespace Rethought.Commands.Actions
{
    public struct ActionResult
    {
        public ActionResult(State state)
        {
            State = state;
        }

        public State State { get; }

        public static ActionResult Completed => new ActionResult(State.Completed);

        public static ActionResult Failed => new ActionResult(State.Failed);
    }
}