using Automaton.Core.Enums;
using Automaton.Core.Interfaces;
using System.Linq.Expressions;

namespace Automaton.Core.Models
{
    public abstract class WorkflowStep
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public string StepType { get; set; }

        public virtual List<string> Children { get; set; } = new List<string>();

        public virtual List<IStepParameter> Inputs { get; set; } = new List<IStepParameter>();

        public virtual List<IStepParameter> Outputs { get; set; } = new List<IStepParameter>();

        public virtual WorkflowErrorHandling? ErrorBehavior { get; set; }

        public virtual TimeSpan? RetryInterval { get; set; }

        public virtual string? CompensationStepId { get; set; }

        public virtual bool ResumeChildrenAfterCompensation => true;

        public virtual bool RevertChildrenAfterCompensation => false;

        public virtual LambdaExpression CancelCondition { get; set; }

        public bool ProceedOnCancel { get; set; } = false;

        public abstract Task<ExecutionResult> RunAsync(IStepExecutionContext context);
    }
}
