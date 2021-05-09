using Elsa.Models;
using Elsa.Persistence.Specifications;
using System;
using System.Linq.Expressions;

namespace Automaton.Studio.Specifications
{
    public class WorkflowDefinitionNameSpecification : Specification<WorkflowDefinition>
    {
        public string Name { get; set; }

        public WorkflowDefinitionNameSpecification(string name)
        {
            Name = name;
        }

        public override Expression<Func<WorkflowDefinition, bool>> ToExpression() => x => x.Name.ToLower() == Name.ToLower();
    }
}