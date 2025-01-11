using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MVCPractice.Attributes
{
    public class ValidateUserIdAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var routeId = context.RouteData.Values["Id"]?.ToString();
            var loggedInUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (loggedInUserId == null || loggedInUserId != routeId)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
