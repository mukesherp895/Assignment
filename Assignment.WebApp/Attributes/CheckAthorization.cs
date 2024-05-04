using Assignment.Model.DTO;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Assignment.Common;

namespace Assignment.WebApp.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CheckAthorization : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool isAjax = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            try
            {
                var userName = context.HttpContext.Session.GetString("UserName");
                if (!isAjax && userName == null)
                {
                    context.Result = new RedirectToActionResult("Login", "Account", new { area = "" });
                    return;
                }
                else if(!isAjax && userName == null)
                {
                    ResponseDto responseDto = new ResponseDto()
                    {
                        Status = ResponseStatus.UnAuthorized,
                        Message = "UnAuthorized"
                    };
                    context.Result = new JsonResult(responseDto);
                    return;
                }
            }
            catch
            {
                if (!isAjax)
                {
                    context.Result = new RedirectToActionResult("Login", "Account", new { area = "" });
                    return;
                }
                else
                {
                    ResponseDto responseDto = new ResponseDto()
                    {
                        Status = ResponseStatus.UnAuthorized,
                        Message = "UnAuthorized"
                    };
                    context.Result = new JsonResult(responseDto);
                    return;
                }
            }

        }
    }
}
