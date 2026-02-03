using Microsoft.AspNetCore.Identity;

namespace dotnet_db.Models;



public class AppUser : IdentityUser<int>
{
    public string FullName { get; set; } = null!;
}