using OnlineShoppingPlatform.DAL.Enums;
using System.ComponentModel.DataAnnotations;
namespace OnlineShoppingPlatform.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }// Kullanıcı kimlik numarası (primary key).

        [Required(ErrorMessage = "Ad alanı zorunludur.")]// Zorunlu alan ve maksimum 50 karakter sınırı.
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir.")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]// Zorunlu alan ve maksimum 50 karakter sınırı.
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir.")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]// Zorunlu alan ve e-posta formatı kontrolü.
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        public string Email { get; set; } = "";

        [Phone(ErrorMessage = "Geçersiz telefon numarası.")]
        public string PhoneNumber { get; set; } = "";

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; } = "";// Kullanıcının şifresi (şifrelenmiş olacak).

        [Required(ErrorMessage = "Rol alanı zorunludur.")]
        public UserRole Role { get; set; }// Kullanıcının rolü (Admin veya Customer).
    }

}
