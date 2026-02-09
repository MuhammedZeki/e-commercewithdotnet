using System.ComponentModel.DataAnnotations;
namespace dotnet_db.Models;



public class OrderCreateModel
{
    [Display(Name = "Ad Soyad")]
    [Required(ErrorMessage = "Lütfen adınızı ve soyadınızı giriniz.")]
    public string Fullname { get; set; } = null!;

    [Display(Name = "E-Posta Adresi")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Telefon Numarası")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    public string Phone { get; set; } = null!;

    [Display(Name = "Toplam Tutar")]
    public double TotalPrice { get; set; }

    [Display(Name = "Şehir")]
    [Required(ErrorMessage = "Lütfen şehir seçiniz.")]
    public string City { get; set; } = null!;

    [Display(Name = "Açık Adres")]
    [Required(ErrorMessage = "Lütfen tam adresinizi yazınız.")]
    public string Address { get; set; } = null!;

    [Display(Name = "Sipariş Notu")]
    public string OrderNote { get; set; } = null!;

    [Display(Name = "Posta Kodu")]
    [Required(ErrorMessage = "Posta kodunu giriniz.")]
    public string PostalCode { get; set; } = null!;
}