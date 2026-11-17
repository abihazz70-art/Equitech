namespace Equitech.Api.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    public const string CorrelationIdHeader = "X-Correlation-ID";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId) || 
            string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("D");
        }

        context.TraceIdentifier = correlationId!;
        context.Response.Headers[CorrelationIdHeader] = correlationId;
        context.Items["CorrelationId"] = correlationId;

        await _next(context);
    }
}
