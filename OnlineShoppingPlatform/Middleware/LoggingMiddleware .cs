using System.Diagnostics;// Stopwatch sınıfı ile işlem süresini ölçmek için gerekli

namespace OnlineShoppingPlatform.API.Middleware
{
    public class LoggingMiddleware// Gelen ve giden HTTP isteklerini loglamak için oluşturulmuş middleware sınıfı
    {
        private readonly RequestDelegate _next; // Middleware zincirindeki bir sonraki middleware'i temsil eder

        // Constructor: Middleware'in çalışması için bir sonraki middleware'in referansını alır
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Asenkron metod: Gelen HTTP isteği işlendiğinde çalışır
        public async Task InvokeAsync(HttpContext context)
        {
            // İstek bilgilerini logla
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;

            // [Request]: HTTP metodunu (GET, POST, vb.), yolunu (path) ve istek zamanını loglar
            Console.WriteLine($"[Request] {request.Method} {request.Path} at {DateTime.Now}");

            // Sonraki middleware'e devam et
            await _next(context);

            // Yanıt bilgilerini logla
            stopwatch.Stop();
            Console.WriteLine($"[Response] {context.Response.StatusCode} in {stopwatch.ElapsedMilliseconds}ms");
            // [Response]: Yanıtın durum kodunu (örneğin, 200, 404) ve işlem süresini loglar
        }
    }
}
