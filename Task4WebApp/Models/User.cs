using System.ComponentModel.DataAnnotations;

namespace Task4WebApp.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty; 

    public DateTime RegistrationTime { get; set; } = DateTime.UtcNow;
    
    public DateTime? LastLoginTime { get; set; }

    public string Status { get; set; } = "Active"; 
}
