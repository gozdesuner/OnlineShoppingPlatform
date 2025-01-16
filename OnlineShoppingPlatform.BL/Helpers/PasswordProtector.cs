using Microsoft.AspNetCore.DataProtection;

namespace OnlineShoppingPlatform.BL.Helpers
{
    public class PasswordProtector
    {
        private readonly IDataProtector _dataProtector;

        public PasswordProtector(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector("OnlineShoppingPlatform.PasswordProtector");
        }

        public string Protect(string plainPassword)
        {
            return _dataProtector.Protect(plainPassword); // Şifreleme
        }

        public string Unprotect(string encryptedPassword)
        {
            return _dataProtector.Unprotect(encryptedPassword); // Şifre çözme
        }
    }
}
