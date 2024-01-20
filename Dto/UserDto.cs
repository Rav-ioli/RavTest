using System.ComponentModel.DataAnnotations;

namespace MyApplication.Dto;

public class UserDto
{
     public string Username { get; set; }
  public string Email { get; set; }

     public bool TwoFactorEnabled { get; set; }

    public bool? NieuwsBriefEnabled { get; set; }
    public string? Phone { get; set; }
    public bool? CookiesEnabled { get; set; }

    public string[]? claims { get; set; }
}

public class UserDtoAdmin : UserDto
{
    public string BedrijfsNaam { get; set; }

    public string Id { get; set; }
}

public class NewUserDtoAdmin : UserDtoAdmin
{
    public string UnhashedPassword { get; set; }
}
