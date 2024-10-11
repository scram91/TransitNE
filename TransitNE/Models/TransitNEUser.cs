using Microsoft.AspNetCore.Identity;

namespace TransitNE.Models;

// Add profile data for application users by adding properties to the TransitNEUser class
public class TransitNEUser : IdentityUser
{
    [PersonalData]
    public string? FirstName { get; set; }

    [PersonalData]
    public string? LastName { get; set; }
}

