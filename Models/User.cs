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
    public string? BearerTokenHash { get; set; }
    public DateTime? BearerTokenExpiresAt { get; set; }
}

/*
dotnet ef migrations add CreateUsers
dotnet ef database update
*/
