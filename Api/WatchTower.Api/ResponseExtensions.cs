using Microsoft.AspNetCore.Mvc;
using WatchTower.Domain.Shared;

namespace WatchTower.Api;

public static class ResponseExtensions
{
    public static ActionResult ToResponse(this ErrorList errorList)
    {
        if (!errorList.Any())
        {
            return new ObjectResult(Envelope.Error(errorList))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        var distinctErrorTypes = errorList
            .Select(x => x.Type)
            .Distinct()
            .ToList();

        var statusCode = distinctErrorTypes.Count > 1
            ? StatusCodes.Status500InternalServerError
            : GetStatusCode(distinctErrorTypes.First());

        var envelope = Envelope.Error(errorList);

        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }

    private static int GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.AlreadyExists => StatusCodes.Status409Conflict,
        ErrorType.Failure => StatusCodes.Status500InternalServerError,

        _ => StatusCodes.Status500InternalServerError
    };
}