using Microsoft.AspNetCore.Authentication.JwtBearer; // JWT kimlik doðrulama için gerekli kütüphane
using Microsoft.EntityFrameworkCore; // Entity Framework Core ile veritabaný iþlemleri için kullanýlýr
using Microsoft.IdentityModel.Tokens; // JWT token doðrulama ve oluþturma için kullanýlýr
using OnlineShoppingPlatform.API.Filters;
using OnlineShoppingPlatform.API.Middleware;
using OnlineShoppingPlatform.BL.Helpers;
using OnlineShoppingPlatform.BL.Interfaces; // Business Logic katmanýndaki arayüzleri kullanmak için
using OnlineShoppingPlatform.BL.Services; // Business Logic katmanýndaki servis sýnýflarýný kullanmak için
using OnlineShoppingPlatform.DAL; // Veri eriþim katmanýndaki sýnýflarý kullanmak için
using OnlineShoppingPlatform.DAL.UnitOfWork; // Unit of Work deseni ile veri eriþim iþlemleri için
using System.Text; // Metin iþlemleri, özellikle JWT anahtarýný byte dizisine dönüþtürmek için

var builder = WebApplication.CreateBuilder(args); // Uygulama yapýlandýrmasýný baþlatýr, temel ayarlarý yükler

// Veritabaný baðlantýsýný yapýlandýr
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // PostgreSQL baðlantýsýný kur

// JWT Ayarlarýný Al
var jwtSettings = builder.Configuration.GetSection("Jwt"); // appsettings.json içindeki "Jwt" bölümünü al
var keyString = jwtSettings["Key"]; // "Key" deðerini al

// Eðer Key eksikse veya boþsa hata fýrlat
if (string.IsNullOrEmpty(keyString) || keyString.Length < 16)
{
    throw new InvalidOperationException("JWT 'Key' deðeri en az 16 karakter uzunluðunda olmalýdýr.");
}

var key = Encoding.UTF8.GetBytes(keyString); // Anahtarý byte dizisine dönüþtür

// Service container'a servisleri ekle
builder.Services.AddScoped<IUserService, UserService>(); // Business Logic katmanýndaki UserService'i baðla
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Data Access katmanýndaki UnitOfWork'u baðla
builder.Services.AddControllers(); // Controller'lar için gerekli servisi ekle
builder.Services.AddScoped<ITokenService, TokenService>();

//var keysDirectory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys"));
builder.Services.AddDataProtection();
//   .PersistKeysToFileSystem(keysDirectory)
// .SetApplicationName("OnlineShoppingPlatform");
builder.Services.AddScoped<PasswordProtector>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<TimeRestrictedAccessFilter>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<MaintenanceState>();



// JWT Kimlik Doðrulama Servisini Ekleyin
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // JWT'yi kimlik doðrulama yöntemi olarak ayarla
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // JWT'yi kimlik doðrulama challenge yöntemi olarak ayarla
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Issuer doðrulamasýný etkinleþtir
        ValidateAudience = true, // Audience doðrulamasýný etkinleþtir
        ValidateLifetime = true, // Token süresini kontrol et
        ValidateIssuerSigningKey = true, // Token imzasýný kontrol et
        ValidIssuer = jwtSettings["Issuer"], // Doðru Issuer deðeri
        ValidAudience = jwtSettings["Audience"], // Doðru Audience deðeri
        IssuerSigningKey = new SymmetricSecurityKey(key) // Token doðrulama için kullanýlan þifreleme anahtarý
    };
});

// Swagger/OpenAPI yapýlandýrmasý
builder.Services.AddEndpointsApiExplorer(); // API endpoint'lerini keþfetmek için
// Swagger belgelerini oluþturmak için
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var app = builder.Build(); // Uygulama yapýlandýrmasýný tamamla

app.UseMiddleware<GlobalExceptionMiddleware>(); // Hatalarý yakalar ve yönetir.
app.UseMiddleware<MaintenanceMiddleware>(); // Önce bakým kontrolü
app.UseMiddleware<LoggingMiddleware>(); // Sonra loglama

// Geliþtirme ortamýnda Swagger etkinleþtir
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger endpoint'lerini etkinleþtir
    app.UseSwaggerUI(); // Swagger kullanýcý arayüzünü etkinleþtir
}

app.UseHttpsRedirection(); // HTTP isteklerini HTTPS'e yönlendir

// Middleware kullaným sýralamasý
app.UseAuthentication(); // JWT kimlik doðrulama middleware
app.UseAuthorization(); // Yetkilendirme middleware

app.MapControllers(); // API controller'larýný endpoint'lere baðla

app.Run(); // Uygulamayý baþlat
