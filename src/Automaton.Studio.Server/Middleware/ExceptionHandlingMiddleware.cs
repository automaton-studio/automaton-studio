using Serilog;
using System.Net;
using System.Text.Json;

namespace Automaton.Studio.Server.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly Serilog.ILogger logger;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
        logger = Log.ForContext<ExceptionHandlingMiddleware>();
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.Error(exception, "{Message}", exception.Message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorModel = new ExceptionResponseModel
        {
            ResponseCode = (int)HttpStatusCode.InternalServerError,
            ResponseMessage = "Internal Server Error. Please retry or contact support if error persists"
        };

        var exResult = JsonSerializer.Serialize(errorModel);

        await context.Response.WriteAsync(exResult);
    }
}
