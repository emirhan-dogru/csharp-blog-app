using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string? UserName { get; set; }

        [Required]
        [Display(Name = "Ad Soyad")]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Eposta")]
        public string? Email { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Mak. 10 karakter belirtiniz.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string? Password { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Mak. 10 karakter belirtiniz.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola Onayla")]
        [Compare(nameof(Password), ErrorMessage = "Parolanız eşleşmiyor")]
        public string? ConfirmPassword { get; set; }
    }
}