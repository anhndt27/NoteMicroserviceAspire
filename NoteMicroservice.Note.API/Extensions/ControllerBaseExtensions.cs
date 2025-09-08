using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace NoteMicroservice.Note.API.Extensions
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
        
        public static List<string> GetGroupIds(this ControllerBase controller)
        {
            return controller.User.FindAll(ClaimTypes.GroupSid)
                .Select(c => c.Value)
                .ToList();
        }
    }
}
