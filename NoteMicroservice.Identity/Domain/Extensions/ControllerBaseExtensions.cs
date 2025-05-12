using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace NoteMicroservice.Identity.Domain.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static ObjectResult InternalServerError(this ControllerBase controller, object value)
        {
            return controller.StatusCode(StatusCodes.Status500InternalServerError, value);
        }

        public static string GetUserId(this ControllerBase controller)
        {
            return controller.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
