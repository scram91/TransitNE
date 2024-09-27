using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TransitNE.Data;
public class TransitNEUser 
{
    public string Id { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public string? UserName { get; set; }
    [PersonalData]
    public string? FirstName { get; set; }
    [PersonalData]
    public string? LastName { get; set; }
}

