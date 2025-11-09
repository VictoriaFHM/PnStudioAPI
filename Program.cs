using Microsoft.EntityFrameworkCore;
using PnStudioAPI.Data;
using PnStudioAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// --- Swagger UI ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddSingleton<ComputeService>();

// EF Core
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection");
    opt.UseSqlServer(cs);
});

var app = builder.Build();

// --- Swagger UI ---
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PnStudioAPI v1");
    c.RoutePrefix = "swagger";
});

// (Si decides seguir usando OpenAPI “nuevo”, deja además: )
// builder.Services.AddOpenApi();
// if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();