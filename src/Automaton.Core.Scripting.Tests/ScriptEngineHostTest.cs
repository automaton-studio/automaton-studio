using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Automaton.Core.Scripting.Tests
{
    [TestClass]
    public class ScriptEngineHostTest
    {
        [TestMethod]
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

            Assert.IsTrue(variables.Any(x => x.Key == "a"));
            Assert.IsTrue(variables.Any(x => x.Key == "b"));
        }

        [TestMethod]
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

            Assert.IsTrue(variables.Any(x => x.Key == "c" && x.Value == 3));
        }
    }
}