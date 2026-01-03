using System.Diagnostics;
using CreditAppManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CreditAppManager.Api.Filters
{
    public class ModelValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
                var instance = context.HttpContext.Request.Path;

                var errorResponse = ApiErrorResponse.ValidationError(errors, instance, traceId);

                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }
    }
}
