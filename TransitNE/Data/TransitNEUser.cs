using Microsoft.AspNetCore.Identity;

namespace TransitNE.Data;
public class TransitNEUser : IdentityUser
{
    [PersonalData]
    public string? FirstName { get; set; }

    [PersonalData]
    public string? LastName { get; set; }
}

