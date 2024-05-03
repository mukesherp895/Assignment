using Azure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Assignment.Model.DTO;
using Assignment.Common;

namespace Assignment.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizationFilter : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                ResponseDto resp = new ResponseDto
                {
                    Message = "Session Expired",
                    Status = ResponseStatus.Fail
                };
                context.Result = new JsonResult(resp) { StatusCode = StatusCodes.Status200OK };
            }
        }
    }
}
