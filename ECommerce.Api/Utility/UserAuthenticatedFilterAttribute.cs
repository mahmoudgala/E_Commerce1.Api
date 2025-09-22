using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.Api.Utility
{
    public class UserAuthenticatedFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Index", "Home", new { area = SD.CustomerArea });

            }
        }
    }
}
