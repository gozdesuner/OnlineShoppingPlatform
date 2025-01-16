namespace OnlineShoppingPlatform.API.Middleware
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;

        public MaintenanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Bakım modunu kontrol et
            var isMaintenanceMode = false; // Bunu dinamik yapmak için veritabanı veya config kullanılabilir
            if (isMaintenanceMode)
            {
                context.Response.StatusCode = 503; // Service Unavailable
                await context.Response.WriteAsync("Uygulama bakım modunda, lütfen daha sonra tekrar deneyin.");
                return;
            }

            await _next(context); // Devam et
        }
    }
}
