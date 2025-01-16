using Microsoft.Extensions.Configuration; // IConfiguration için gerekli
using Microsoft.IdentityModel.Tokens; // SymmetricSecurityKey ve SigningCredentials için gerekli
using OnlineShoppingPlatform.BL.Interfaces; // ITokenService arayüzü için gerekli
using OnlineShoppingPlatform.DAL.Entities; // User sınıfı için gerekli
using System.IdentityModel.Tokens.Jwt; // JwtSecurityToken ve JwtSecurityTokenHandler için gerekli
using System.Security.Claims; // Claim sınıfı için gerekli
using System.Text; // Encoding işlemleri için gerekli

namespace OnlineShoppingPlatform.BL.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // JWT ayarlarını appsettings.json'dan alıyoruz.
        public string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var keyString = jwtSettings["Key"];

            // Eğer key eksikse veya boşsa hata fırlat
            if (string.IsNullOrEmpty(keyString))
            {
                throw new InvalidOperationException("JWT 'Key' değeri appsettings.json dosyasından alınamadı veya eksik.");
            }

            // Anahtarı oluştur
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Kullanıcı bilgileri için claim oluştur
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),// Kullanıcının email bilgisi.
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),// Unique bir ID.
                new Claim(ClaimTypes.Role, user.Role.ToString()),// Kullanıcının rolü.
                new Claim("UserId", user.Id.ToString())// Kullanıcının kimliği.
            };

            // ExpiresInMinutes kontrolü
            var expiresInMinutes = jwtSettings["ExpiresInMinutes"];
            if (!double.TryParse(expiresInMinutes, out double minutes))
            {
                minutes = 60; // Varsayılan süre
            }

            // JWT oluştur
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),//Token süreleri her zaman UTC'ye göre ayarlanmalıdır, aksi takdirde farklı zaman dilimlerinde problemler oluşabilir. 
                signingCredentials: creds
            );

            // Token döndür
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
