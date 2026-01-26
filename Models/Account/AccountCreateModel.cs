using System.ComponentModel.DataAnnotations;

namespace dotnet_db.Models;


public class AccountCreateModel
{
    [Display(Name = "Kullanıcı Adı")]
    [Required(ErrorMessage = "Lütfen kullanıcı adı giriniz!")]
    public string UserName { get; set; } = null!;

    [Display(Name = "E-Posta")]
    [Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz!")]
    [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz!")]
    public string Email { get; set; } = null!;


    [Display(Name = "Şifre")]
    [Required(ErrorMessage = "Lütfen bir şifre giriniz!")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Şifre Tekrar")]
    [Required(ErrorMessage = "Lütfen şifrenizi tekrar giriniz!")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Şifreler birbiriyle uyuşmuyor!")]
    public string ConfirmPassword { get; set; } = null!;
}