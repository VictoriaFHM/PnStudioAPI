using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PnStudioAPI.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public int Id { get; set; }

    [Required, MaxLength(160), EmailAddress]
    public string Email { get; set; } = null!;

    [MaxLength(120)]
    public string? Name { get; set; }

    // Para futura auth (opcional en MVP)
    [MaxLength(256)]
    public string? PasswordHash { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public List<Project> Projects { get; set; } = new();
}