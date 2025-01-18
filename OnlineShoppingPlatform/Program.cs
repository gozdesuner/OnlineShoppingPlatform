using Microsoft.AspNetCore.Authentication.JwtBearer; // JWT kimlik do�rulama i�in gerekli k�t�phane
using Microsoft.EntityFrameworkCore; // Entity Framework Core ile veritaban� i�lemleri i�in kullan�l�r
using Microsoft.IdentityModel.Tokens; // JWT token do�rulama ve olu�turma i�in kullan�l�r
using OnlineShoppingPlatform.API.Filters;
using OnlineShoppingPlatform.API.Middleware;
using OnlineShoppingPlatform.BL.Helpers;
using OnlineShoppingPlatform.BL.Interfaces; // Business Logic katman�ndaki aray�zleri kullanmak i�in
using OnlineShoppingPlatform.BL.Services; // Business Logic katman�ndaki servis s�n�flar�n� kullanmak i�in
using OnlineShoppingPlatform.DAL; // Veri eri�im katman�ndaki s�n�flar� kullanmak i�in
using OnlineShoppingPlatform.DAL.UnitOfWork; // Unit of Work deseni ile veri eri�im i�lemleri i�in
using System.Text; // Metin i�lemleri, �zellikle JWT anahtar�n� byte dizisine d�n��t�rmek i�in

var builder = WebApplication.CreateBuilder(args); // Uygulama yap�land�rmas�n� ba�lat�r, temel ayarlar� y�kler

// Veritaban� ba�lant�s�n� yap�land�r
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // PostgreSQL ba�lant�s�n� kur

// JWT Ayarlar�n� Al
var jwtSettings = builder.Configuration.GetSection("Jwt"); // appsettings.json i�indeki "Jwt" b�l�m�n� al
var keyString = jwtSettings["Key"]; // "Key" de�erini al

// E�er Key eksikse veya bo�sa hata f�rlat
if (string.IsNullOrEmpty(keyString) || keyString.Length < 16)
{
    throw new InvalidOperationException("JWT 'Key' de�eri en az 16 karakter uzunlu�unda olmal�d�r.");
}

var key = Encoding.UTF8.GetBytes(keyString); // Anahtar� byte dizisine d�n��t�r

// Service container'a servisleri ekle
builder.Services.AddScoped<IUserService, UserService>(); // Business Logic katman�ndaki UserService'i ba�la
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Data Access katman�ndaki UnitOfWork'u ba�la
builder.Services.AddControllers(); // Controller'lar i�in gerekli servisi ekle
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



// JWT Kimlik Do�rulama Servisini Ekleyin
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // JWT'yi kimlik do�rulama y�ntemi olarak ayarla
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // JWT'yi kimlik do�rulama challenge y�ntemi olarak ayarla
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Issuer do�rulamas�n� etkinle�tir
        ValidateAudience = true, // Audience do�rulamas�n� etkinle�tir
        ValidateLifetime = true, // Token s�resini kontrol et
        ValidateIssuerSigningKey = true, // Token imzas�n� kontrol et
        ValidIssuer = jwtSettings["Issuer"], // Do�ru Issuer de�eri
        ValidAudience = jwtSettings["Audience"], // Do�ru Audience de�eri
        IssuerSigningKey = new SymmetricSecurityKey(key) // Token do�rulama i�in kullan�lan �ifreleme anahtar�
    };
});

// Swagger/OpenAPI yap�land�rmas�
builder.Services.AddEndpointsApiExplorer(); // API endpoint'lerini ke�fetmek i�in
// Swagger belgelerini olu�turmak i�in
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


var app = builder.Build(); // Uygulama yap�land�rmas�n� tamamla

app.UseMiddleware<GlobalExceptionMiddleware>(); // Hatalar� yakalar ve y�netir.
app.UseMiddleware<MaintenanceMiddleware>(); // �nce bak�m kontrol�
app.UseMiddleware<LoggingMiddleware>(); // Sonra loglama

// Geli�tirme ortam�nda Swagger etkinle�tir
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger endpoint'lerini etkinle�tir
    app.UseSwaggerUI(); // Swagger kullan�c� aray�z�n� etkinle�tir
}

app.UseHttpsRedirection(); // HTTP isteklerini HTTPS'e y�nlendir

// Middleware kullan�m s�ralamas�
app.UseAuthentication(); // JWT kimlik do�rulama middleware
app.UseAuthorization(); // Yetkilendirme middleware

app.MapControllers(); // API controller'lar�n� endpoint'lere ba�la

app.Run(); // Uygulamay� ba�lat
