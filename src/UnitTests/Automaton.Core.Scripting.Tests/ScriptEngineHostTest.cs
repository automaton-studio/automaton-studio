namespace Automaton.Core.Scripting.Tests
{
    public class ScriptEngineHostTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Description("Test that input variables are returned to execution result")]
        public void InputVariables()
        {
            var factory = new ScriptEngineFactory();
            var scriptEngine = new ScriptEngineHost(factory);
            var resource = new ScriptResource()
            {
                ContentType = @"text/x-python",
                Content = string.Empty
            };
            var inputs = new Dictionary<string, object>()
            {
                {"a", 1 },
                {"b", 2 }
            };

            var variables = scriptEngine.Execute(resource, inputs);

            Assert.That(variables.Any(x => x.Key == "a"), Is.True);
            Assert.That(variables.Any(x => x.Key == "b"), Is.True);
        }

        [Test]
        [Description("Test that Python variables are returned to execution result")]
        public void OutputVariables()
        {
            var factory = new ScriptEngineFactory();
            var scriptEngine = new ScriptEngineHost(factory);
            var resource = new ScriptResource()
            {
                ContentType = @"text/x-python",
                Content = "c = a + b;"
            };
            var inputs = new Dictionary<string, object>()
            {
                {"a", 1 },
                {"b", 2 }
            };

            var variables = scriptEngine.Execute(resource, inputs);

            Assert.That(variables.Any(x => x.Key == "c" && x.Value == 3), Is.True);
        }
    }
}