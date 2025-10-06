using GatewayService.Web.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GatewayService.Web.Api;

public class ValidationFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = new Dictionary<string, string>();
                
            foreach (var state in context.ModelState)
            {
                if (state.Value.Errors.Count > 0)
                {
                    errors[state.Key] = string.Join("; ", state.Value.Errors.Select(e => e.ErrorMessage));
                }
            }

            var response = new ValidationErrorResponseDto
            {
                Message = "One or more validation errors occurred",
                Errors = errors
            };

            context.Result = new BadRequestObjectResult(response);
        }
    }
}