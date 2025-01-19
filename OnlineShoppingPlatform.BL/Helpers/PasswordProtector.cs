using Microsoft.AspNetCore.DataProtection; // Data Protection için gerekli namespace

namespace OnlineShoppingPlatform.BL.Helpers // OnlineShoppingPlatform için yardımcı sınıflar
{
    public class PasswordProtector // Şifreleme ve çözme işlemleri için bir sınıf
    {
        private readonly IDataProtector _dataProtector; // Şifreleme ve çözme işlemlerini gerçekleştiren koruyucu

        public PasswordProtector(IDataProtectionProvider dataProtectionProvider) // Constructor: IDataProtectionProvider ile koruyucu oluşturur
        {
            _dataProtector = dataProtectionProvider.CreateProtector("OnlineShoppingPlatform"); // "OnlineShoppingPlatform" amacı ile bir koruyucu oluşturur
        }

        public string Protect(string plainPassword) // Şifreleme işlemi için metod
        {
            string korunmus = _dataProtector.Protect(plainPassword); // Şifrelenmiş (korunmuş) veriyi üretir
            string normal = _dataProtector.Unprotect(korunmus); // Korunan şifreyi çözerek eski haline getirir (test için)
            return _dataProtector.Protect(plainPassword); // Şifreleme işlemini tekrar döndürür
        }

        public string Unprotect(string encryptedPassword) // Şifre çözme işlemi için metod
        {
            return _dataProtector.Unprotect(encryptedPassword); // Şifrelenmiş (korunmuş) veriyi çözerek düz metin haline getirir
        }

        public bool ValidatePassword(string inputPassword, string encryptedPassword) // Şifre doğrulama işlemi
        {
            try
            {
                var decryptedPassword = Unprotect(encryptedPassword); // Şifrelenmiş şifreyi çözer
                return decryptedPassword == inputPassword; // Çözülen şifre ile kullanıcıdan gelen şifre eşleşiyor mu kontrol eder
            }
            catch
            {
                return false; // Şifre çözme sırasında hata olursa (örneğin geçersiz bir şifre çözmeye çalışılırsa) false döner
            }
        }
    }
}
