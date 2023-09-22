namespace BadgerHTN
{
    /*
        A wrapper base class that makes it easier to add new action
        nodes with a custom context.
    */
    public class CustomNode<T> : ActionNode where T : BaseContext
    {
        public override State Execute(IContext context)
        {
            if (context is T ctx)
            {
                // Wrap generic context
                return OnExecute(ctx);
            }

            return State.Success;
        }

        public override void Interrupt(IContext context)
        {
            if (context is T ctx)
            {
                // Wrap generic context
                OnInterrupt(ctx);
            }
        }

        public override bool Query(Blackboard workingState, IContext context)
        {
            if (context is T ctx)
            {
                return OnQuery(ctx);
            }

            return true;
        }

        protected virtual State OnExecute(T context)
        {
            // Implement custom OnExecute logic in child class.
            return State.Success;
        }

        /// <summary>
        /// Logic when for a node when a plan was interrupted.
        /// </summary>
        protected virtual void OnInterrupt(T context)
        {
            // Implement custom OnInterrupt logic in child class.
        }

        protected virtual bool OnQuery(T context)
        {
            // Implement custom OnQuery logic in child class.
            return true;
        }
    }
}