using Microsoft.AspNetCore.DataProtection;

namespace OnlineShoppingPlatform.BL.Helpers
{
    public class PasswordProtector
    {
        private readonly IDataProtector _dataProtector;

        public PasswordProtector(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector("OnlineShoppingPlatform");
        }

        public string Protect(string plainPassword)
        {
            string korunmus = _dataProtector.Protect(plainPassword);
            string normal = _dataProtector.Unprotect(korunmus);
            return _dataProtector.Protect(plainPassword); // Şifreleme
        }

        public string Unprotect(string encryptedPassword)
        {
            return _dataProtector.Unprotect(encryptedPassword); // Şifre çözme
        }

        public bool ValidatePassword(string inputPassword, string encryptedPassword)
        {
            try
            {
                var decryptedPassword = Unprotect(encryptedPassword); // Şifre çözme
                return decryptedPassword == inputPassword; // Şifre eşleşiyor mu?
            }
            catch
            {
                return false; // Şifre çözme başarısız olursa
            }
        }
    }
}