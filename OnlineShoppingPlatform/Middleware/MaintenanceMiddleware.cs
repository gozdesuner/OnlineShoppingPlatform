using OnlineShoppingPlatform.BL.Helpers;

namespace OnlineShoppingPlatform.API.Middleware
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MaintenanceState _maintenanceState;

        public MaintenanceMiddleware(RequestDelegate next, MaintenanceState maintenanceState)
        {
            _next = next;
            _maintenanceState = maintenanceState;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Bakım modu kontrolü
            if (_maintenanceState.IsInMaintenance())
            {
                context.Response.StatusCode = 503; // Service Unavailable
                await context.Response.WriteAsync("Uygulama bakım modunda, lütfen daha sonra tekrar deneyin.");
                return;
            }

            await _next(context); // Eğer bakım modu kapalıysa isteği devam ettir
        }
    }
}