
 # Online Shopping Platform

Bu proje, modern bir e-ticaret platformunun backend API'sini içeren kapsamlı bir .NET Core uygulamasıdır.

## 🎯 Proje Amacı

Bu platform, kullanıcıların online alışveriş yapabilmelerini, siparişlerini yönetebilmelerini ve ürünleri inceleyebilmelerini sağlayan bir e-ticaret sistemidir. Proje, N-Tier Architecture prensiplerine uygun olarak geliştirilmiştir.

## 🛠 Kullanılan Teknolojiler

- **.NET 8.0**
- **Entity Framework Core**
- **PostgreSQL**
- **JWT (JSON Web Token) Authentication**
- **Swagger/OpenAPI**
- **Repository Pattern**
- **Unit of Work Pattern**

## 🏗 Proje Yapısı

Proje, üç ana katmandan oluşmaktadır:

### 1. API Layer (OnlineShoppingPlatform.API)
- Controllers
  - OrderController: Sipariş işlemlerini yönetir
  - ProductController: Ürün işlemlerini yönetir
  - UserController: Kullanıcı işlemlerini yönetir
  - AuthController:Login ve Register işlemlerini yapar.
  - MaintenanceController:Bakıma alınma durumunu kontrol eder.
- Middleware
  - Global Exception Handling
  - Maintenance
  - Logging
- Authentication/Authorization
- Swagger Entegrasyonu

### 2. Business Layer (OnlineShoppingPlatform.BL)
- Services
  - OrderService: Sipariş işlemlerinin iş mantığını içerir
  - ProductService: Ürün işlemlerinin iş mantığını içerir
  - UserService: Kullanıcı işlemlerinin iş mantığını içerir
- Interfaces
  - IOrderService
  - IProductService
  - IUserService
- Validation
- DTOs

### 3. Data Access Layer (OnlineShoppingPlatform.DAL)
- Entity Models
  - Order
  - OrderProduct
  - Product
  - User
- DbContext
  - AppDbContext
- Repositories
  - Generic Repository Pattern
- Unit of Work
  - Transaction Management
- Migrations

## 🔑 Temel Özellikler

### Sipariş Yönetimi
```csharp
// Sipariş oluşturma örneği
POST /api/orders
{
    "customerId": 1,
    "orderDate": "2024-03-20T10:00:00",
    "totalAmount": 150.50,
    "productIds": [1, 2, 3]
}
```

### Ürün Yönetimi
- Ürün ekleme, güncelleme, silme
- Ürün listeleme ve filtreleme
- Ürün detayları görüntüleme

### Kullanıcı Yönetimi
- Kullanıcı kaydı ve girişi
- JWT bazlı kimlik doğrulama
- Rol tabanlı yetkilendirme

## 💡 Mimari Özellikler

### Repository Pattern
```csharp
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}
```

### Unit of Work Pattern
```csharp
public interface IUnitOfWork : IDisposable
{
    IRepository<Order> Orders { get; }
    IRepository<Product> Products { get; }
    Task<int> CompleteAsync();
}
```

## 🚀 Kurulum

1. Repository'yi klonlayın
2. PostgreSQL veritabanını oluşturun
3. Connection string'i `appsettings.json` dosyasında güncelleyin:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=OnlineShoppingDB;Username=your_username;Password=your_password"
  }
}
```
4. Migration'ları uygulayın:
```bash
dotnet ef database update
```
5. Projeyi çalıştırın:
```bash
dotnet run
```

## 📝 API Endpoints

### Order Endpoints
- `GET /api/orders`: Tüm siparişleri listele
- `GET /api/orders/{id}`: Sipariş detayı
- `POST /api/orders`: Yeni sipariş oluştur
- `PUT /api/orders/{id}`: Sipariş güncelle
- `DELETE /api/orders/{id}`: Sipariş sil

## 🔒 Güvenlik

- JWT Authentication
- Role-based Authorization
- Input Validation
- CORS Configuration

## 🌟 Gelecek Geliştirmeler

- [ ] Ödeme sistemi entegrasyonu
- [ ] Redis cache implementasyonu
- [ ] Unit test coverage artırımı
- [ ] Logging mekanizmasının geliştirilmesi
- [ ] Performance optimizasyonları

## 📚 Swagger Dokümantasyonu

API dokümantasyonuna aşağıdaki URL üzerinden erişilebilir:
```
https://localhost:7235/swagger
```


```
