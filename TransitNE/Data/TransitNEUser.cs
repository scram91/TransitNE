using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransitNE.Data;

// Add profile data for application users by adding properties to the TransitNEUser class
public class TransitNEUser : IdentityUser
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }
}

