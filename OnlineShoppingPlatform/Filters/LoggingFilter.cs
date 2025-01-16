using Microsoft.AspNetCore.Mvc.Filters; // Action Filter'ları kullanmak için gerekli kütüphane
using System.Diagnostics; // Debug logları için gerekli

namespace OnlineShoppingPlatform.API.Filters // Filtre sınıfımızın bulunduğu namespace
{
    public class LoggingFilter : Attribute, IActionFilter // Attribute ve IActionFilter arayüzünden türetiliyor
    {
        // Bu metod, endpoint çağrılmadan ÖNCE çalışır
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request; // İsteği temsil eden HTTP bilgisi
            var logMessage = $"[REQUEST] {request.Method} {request.Path} - {DateTime.Now}"; // İstek bilgilerini log mesajı olarak oluşturuyoruz
            Debug.WriteLine(logMessage); // Debug penceresine yazıyoruz (isteğe göre dosyaya da yazılabilir)
        }

        // Bu metod, endpoint çağrıldıktan SONRA çalışır
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var response = context.HttpContext.Response; // Yanıtı temsil eden HTTP bilgisi
            var logMessage = $"[RESPONSE] Status Code: {response.StatusCode} - {DateTime.Now}"; // Yanıt bilgilerini log mesajı olarak oluşturuyoruz
            Debug.WriteLine(logMessage); // Debug penceresine yazıyoruz
        }
    }
}
