using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TransitNE.Models;

// Add profile data for application users by adding properties to the TransitNEUser class
[ExcludeFromCodeCoverage]
public class TransitNEUser : IdentityUser
{
    [PersonalData]
    public string? FirstName { get; set; }

    [PersonalData]
    public string? LastName { get; set; }
}