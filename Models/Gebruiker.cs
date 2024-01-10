using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace StichtingAccessibility.Models;

public class Gebruiker : IdentityUser
{
    [NotMapped]
    public bool? EmailConfirmed { get; set; }

    public String testtt { get; set; }
}