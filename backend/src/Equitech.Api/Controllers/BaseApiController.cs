using Microsoft.AspNetCore.Mvc;
using Equitech.Domain.Common;

namespace Equitech.Api.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected string GetCorrelationId()
    {
        return HttpContext.Items["CorrelationId"]?.ToString() 
            ?? HttpContext.TraceIdentifier 
            ?? Guid.NewGuid().ToString("D");
    }

    protected IActionResult ProblemResponse(
        int statusCode, 
        string title, 
        string detail, 
        string errorCode = ErrorCodes.InvalidPayload)
    {
        var correlationId = GetCorrelationId();
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = HttpContext.Request.Path
        };

        problem.Extensions["errorCode"] = errorCode;
        problem.Extensions["correlationId"] = correlationId;
        problem.Extensions["timestamp"] = DateTime.UtcNow.ToString("o");

        return StatusCode(statusCode, problem);
    }

    protected IActionResult BadRequestProblem(string detail, string errorCode = ErrorCodes.InvalidPayload)
    {
        return ProblemResponse(StatusCodes.Status400BadRequest, "Bad Request", detail, errorCode);
    }

    protected IActionResult NotFoundProblem(string detail, string errorCode = ErrorCodes.ResourceNotFound)
    {
        return ProblemResponse(StatusCodes.Status404NotFound, "Not Found", detail, errorCode);
    }

    protected IActionResult ConflictProblem(string detail, string errorCode = ErrorCodes.Conflict)
    {
        return ProblemResponse(StatusCodes.Status409Conflict, "Conflict", detail, errorCode);
    }

    protected IActionResult UnprocessableProblem(string detail, string errorCode = ErrorCodes.InvalidPayload)
    {
        return ProblemResponse(StatusCodes.Status422UnprocessableEntity, "Unprocessable Entity", detail, errorCode);
    }
}
