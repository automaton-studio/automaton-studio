namespace WorkflowCore.Models
{
    public class ExecutionResult
    {
        public bool Proceed { get; set; }

        public static ExecutionResult Next()
        {
            return new ExecutionResult
            {
                Proceed = true,
            };
        }
    }
}
