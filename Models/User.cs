using System.ComponentModel.DataAnnotations;
public class User
{
    public int Id { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string LoginTokenHash { get; set; } = string.Empty;
    public DateTime? LoginTokenExpiresAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public int LoginNumber {get; set; } = 0;
}

/*
dotnet ef migrations add CreateUsers
dotnet ef database update
*/
