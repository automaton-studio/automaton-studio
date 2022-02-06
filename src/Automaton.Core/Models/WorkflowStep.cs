using Automaton.Core.Enums;
using Automaton.Core.Interfaces;
using System.Linq.Expressions;

namespace Automaton.Core.Models
{
    public abstract class WorkflowStep
    {
        public abstract Type BodyType { get; }

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string ExternalId { get; set; }

        public virtual List<int> Children { get; set; } = new List<int>();

        public virtual List<IStepParameter> Inputs { get; set; } = new List<IStepParameter>();

        public virtual List<IStepParameter> Outputs { get; set; } = new List<IStepParameter>();

        public virtual WorkflowErrorHandling? ErrorBehavior { get; set; }

        public virtual TimeSpan? RetryInterval { get; set; }

        public virtual int? CompensationStepId { get; set; }

        public virtual bool ResumeChildrenAfterCompensation => true;

        public virtual bool RevertChildrenAfterCompensation => false;

        public virtual LambdaExpression CancelCondition { get; set; }

        public bool ProceedOnCancel { get; set; } = false;

        public virtual IStepBody ConstructBody(IServiceProvider serviceProvider)
        {
            IStepBody body = (serviceProvider.GetService(BodyType) as IStepBody);
            if (body == null)
            {
                var stepCtor = BodyType.GetConstructor(new Type[] { });
                if (stepCtor != null)
                    body = (stepCtor.Invoke(null) as IStepBody);
            }
            return body;
        }
    }

    public class WorkflowStep<TStepBody> : WorkflowStep
        where TStepBody : IStepBody 
    {
        public override Type BodyType => typeof(TStepBody);
    }
}
