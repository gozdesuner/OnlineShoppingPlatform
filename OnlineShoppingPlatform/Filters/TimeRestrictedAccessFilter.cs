using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineShoppingPlatform.API.Filters
{
    public class TimeRestrictedAccessFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var currentHour = DateTime.Now.Hour;

            // Örneğin, yalnızca 9:00 - 17:00 arası erişime izin ver
            if (currentHour < 1 || currentHour > 23)
            {
                context.Result = new ContentResult
                {
                    StatusCode = 403, // 403 Forbidden
                    Content = "Bu endpoint yalnızca çalışma saatleri içinde kullanılabilir."
                };
            }

        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Action tamamlandıktan sonra yapılacak bir işlem yoksa bu metot boş kalabilir
        }
    }
}


