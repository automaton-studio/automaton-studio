using Automaton.Core.Models;

namespace Automaton.Core.Scripting.Tests
{
    public class ExpressionParserTest
    {
        [Test]
        [Description("Concatenate string variables")]
        public void ConcatenateStringVariables()
        {
            var workflow = new Workflow();
            workflow.Variables.Add("A", new StepVariable { Id = "A", Name = "A", Value = "a", Type = Enums.VariableType.String });
            workflow.Variables.Add("B", new StepVariable { Id = "B", Name = "B", Value = "b", Type = Enums.VariableType.String });

            var stringVariable = new StepVariable { Id = "C", Name = "C", Value = "%A% + %B%", Type = Enums.VariableType.String };
            var result = StepVariableParser.Parse(stringVariable, workflow);

            Assert.That(result, Is.EqualTo("ab"));
        }

        [Test]
        [Description("Concatenate string variables and strings")]
        public void ConcatenateStringVariablesAndStrings()
        {
            var workflow = new Workflow();
            workflow.Variables.Add("A", new StepVariable { Id = "A", Name = "A", Value = "a", Type = Enums.VariableType.String });

            var stringVariable = new StepVariable { Id = "C", Name = "C", Value = "%A% + bc", Type = Enums.VariableType.String };
            var result = StepVariableParser.Parse(stringVariable, workflow);

            Assert.That(result, Is.EqualTo("abc"));
        }

        [Test]
        [Description("Concatenate numeric variables")]
        public void ConcatenateNumberVariables()
        {
            var workflow = new Workflow();
            workflow.Variables.Add("A", new StepVariable { Id = "A", Name = "A", Value = 1m, Type = Enums.VariableType.Number });
            workflow.Variables.Add("B", new StepVariable { Id = "B", Name = "B", Value = 2m, Type = Enums.VariableType.Number });

            var stringVariable = new StepVariable { Id = "C", Name = "C", Value = "%A% + %B%", Type = Enums.VariableType.Number };
            var result = StepVariableParser.Parse(stringVariable, workflow);

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        [Description("Concatenate numeric variables with numbers")]
        public void ConcatenateNumberVariablesWithNumbers()
        {
            var workflow = new Workflow();
            workflow.Variables.Add("A", new StepVariable { Id = "A", Name = "A", Value = 1m, Type = Enums.VariableType.Number });

            var stringVariable = new StepVariable { Id = "C", Name = "C", Value = "%A% + 1", Type = Enums.VariableType.Number };
            var result = StepVariableParser.Parse(stringVariable, workflow);

            Assert.That(result, Is.EqualTo(2));
        }
    }
}
