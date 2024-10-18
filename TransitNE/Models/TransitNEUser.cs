using Microsoft.AspNetCore.Identity;

namespace TransitNE.Models;

// Add profile data for application users by adding properties to the TransitNEUser class
public class TransitNEUser : IdentityUser
{
    public string Id { get; set; }
    
    public string? Email { get; set; }
    public string? UserName { get; set; }
    [PersonalData]
    public string? FirstName { get; set; }
    [PersonalData]
    public string? LastName { get; set; }
}

