using System.ComponentModel.DataAnnotations;

namespace dotnet_db.Models;


public class AccountChangePasswordModel
{

    [Display(Name = "Mevcut Şifre")]
    [Required(ErrorMessage = "Lütfen mevcut şifrenizi giriniz!")]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; } = null!;

    [Display(Name = "Yeni Şifre")]
    [Required(ErrorMessage = "Lütfen bir şifre giriniz!")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Yeni Şifre Tekrar")]
    [Required(ErrorMessage = "Lütfen şifrenizi tekrar giriniz!")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Şifreler birbiriyle uyuşmuyor!")]
    public string ConfirmPassword { get; set; } = null!;

}