
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
// using System.Collections.Generic;

namespace AccessibilityModels;

public class Gebruiker : IdentityUser
{
    [NotMapped]
    public bool? EmailConfirmed { get; set; }
    [NotMapped]
    public string? NormalizedEmail { get; set; }
    [NotMapped]
    public string? NormalizedUserName { get; set; }
    [NotMapped]
    public bool? PhoneNumberConfirmed { get; set; }
    [NotMapped]
    public bool? TwoFactorEnabled { get; set; }

    
    public string? Postcode { get; set; }
}