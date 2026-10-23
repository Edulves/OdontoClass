using Microsoft.AspNetCore.Mvc;
using OdontoClass.Data.ResultPattern.Base;
using OdontoClass.Utils;

namespace OdontoClass.Data.ResultPattern.Extensions;

public static class ResultPatternExtension
{
    public static IActionResult ToIActionResult<T>(this ResultPattern<T> result, ControllerBase controller)
    {
        if (result.IsSuccess)
            return controller.StatusCode(result.StatusCode, result.Value);

        var problem = new ProblemDetailsOdontoClass(result.Title, result.Detail);

        return controller.StatusCode(result.StatusCode, problem);
    }
}
