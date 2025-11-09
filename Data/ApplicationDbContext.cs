using Microsoft.EntityFrameworkCore;
using PnStudioAPI.Models;

namespace PnStudioAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Calculation> Calculations => Set<Calculation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Para el MVP no necesitamos configuración Fluent.
        // Tus DataAnnotations ([Required], [MaxLength], [Index], etc.) ya definen el esquema.
        base.OnModelCreating(modelBuilder);
    }
}