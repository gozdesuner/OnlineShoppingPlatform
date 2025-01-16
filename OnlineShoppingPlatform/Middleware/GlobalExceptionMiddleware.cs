using System.Net; // HTTP durum kodlarını kullanmak için gerekli.
using System.Text.Json; // JSON formatında yanıt oluşturmak için kullanılır.

namespace OnlineShoppingPlatform.API.Middleware
{
    // Global exception handling middleware sınıfı
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next; // Bir sonraki middleware'e geçişi temsil eder.
        private readonly ILogger<GlobalExceptionMiddleware> _logger; // Loglama için kullanılan bir nesne.

        // Constructor: Middleware bağımlılıklarını alır (RequestDelegate ve ILogger).
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next; // Gelen isteği bir sonraki middleware'e iletmek için saklar.
            _logger = logger; // Hata loglaması için kullanılan logger nesnesini saklar.
        }

        // Middleware'in asıl işlemi burada yapılır.
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // İstek sorunsuzsa bir sonraki middleware'e devam et.
                await _next(context);
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama yap ve özel bir yanıt oluştur.
                _logger.LogError(ex, "Beklenmeyen bir hata oluştu."); // Hatayı logla.
                await HandleExceptionAsync(context, ex); // Hata yanıtını yönet.
            }
        }

        // Hataları yakalayıp yanıt oluşturmak için kullanılan özel bir metot.
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json"; // Yanıt türünü JSON olarak ayarla.
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // HTTP durum kodunu 500 (Internal Server Error) yap.

            // Kullanıcıya döndürülecek hata mesajı yapısını oluştur.
            var response = new
            {
                StatusCode = context.Response.StatusCode, // HTTP durum kodu.
                Message = "Sunucuda bir hata oluştu. Lütfen tekrar deneyin.", // Kullanıcı dostu hata mesajı.
                Detailed = exception.Message // Geliştirme sırasında detaylı hata mesajı. Prod'da kaldırılabilir.
            };

            // Hata mesajını JSON formatına dönüştür.
            var json = JsonSerializer.Serialize(response);

            // Yanıt olarak JSON'u döndür.
            return context.Response.WriteAsync(json);
        }
    }
}
