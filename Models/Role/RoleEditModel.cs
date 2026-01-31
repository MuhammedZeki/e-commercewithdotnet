using System.ComponentModel.DataAnnotations;

namespace dotnet_db.Models;


public class RoleEditModel
{

    public int Id { get; set; }


    [Display(Name = "Rol Adı")]
    [Required(ErrorMessage = "Lütfen rol adını giriniz!")]
    [StringLength(30)]
    public string RoleName { get; set; } = null!;
}

