using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PnStudioAPI.Models;

[Index(nameof(UserId), nameof(Name), IsUnique = true)]
public class Project
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = null!;

    [MaxLength(2000)]
    public string? Notes { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public List<Calculation> Calculations { get; set; } = new();
}