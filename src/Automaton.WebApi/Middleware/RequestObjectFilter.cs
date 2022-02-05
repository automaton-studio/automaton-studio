using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System.Dynamic;

namespace Automaton.WebApi.Middleware
{
    public class RequestObjectFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {            
            foreach (var key in context.ActionArguments.Keys.ToList())
            {
                if (context.ActionArguments[key] is JObject)
                    context.ActionArguments[key] = (context.ActionArguments[key] as JObject).ToObject<ExpandoObject>();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
