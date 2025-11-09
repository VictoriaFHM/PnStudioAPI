using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PnStudioAPI.Data;
using PnStudioAPI.Dto;
using PnStudioAPI.Models;
using PnStudioAPI.Services;

namespace PnStudioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class CalculationsController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ComputeService _svc;

    public CalculationsController(ApplicationDbContext db, ComputeService svc)
    {
        _db = db;
        _svc = svc;
    }

    // POST /api/calculations  -> guarda un cálculo (sin usuarios/proyectos obligatorios)
    [HttpPost]
    [ProducesResponseType(typeof(CalculationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CalculationDto>> Create([FromBody] SaveCalculationRequest req, CancellationToken ct)
    {
        if (req.Vth <= 0 || req.Rth <= 0)
            return BadRequest("Vth y Rth deben ser > 0.");

        var pmax = CoreMath.PMax(req.Vth, req.Rth);
        var (ok, k, c, err) = NormalizeKc(req, pmax);
        if (!ok) return BadRequest(err);

        // Ejecuta el cálculo con el servicio
        var comp = _svc.Compute(new ComputeRequest { Vth = req.Vth, Rth = req.Rth, K = k, C = c });

        // Persiste
        var entity = new Calculation
        {
            // ProjectId es opcional (debe ser int? en el modelo)
            ProjectId     = req.ProjectId,
            Vth           = req.Vth,
            Rth           = req.Rth,
            K             = k,
            C             = c,
            Pmax          = comp.Pmax,
            RlMin         = comp.RlMin,
            RlMax         = comp.RlMax,
            RecommendedRl = comp.RecommendedRl,
            EtaAtRec      = comp.EtaAtRec,
            PAtRec        = comp.PAtRec,
            PMin          = comp.PMin,
            PMaxByK       = comp.PMaxByK,
            Regime        = GuessRegime(req.Rth, comp.RecommendedRl),
            CreatedAtUtc  = DateTime.UtcNow
        };

        _db.Calculations.Add(entity);
        await _db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, ToDto(entity));
    }

    // GET /api/calculations/{id}
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(CalculationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CalculationDto>> GetById(long id, CancellationToken ct)
    {
        var e = await _db.Calculations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (e is null) return NotFound();
        return Ok(ToDto(e));
    }

    // GET /api/calculations?take=20
    [HttpGet]
    [ProducesResponseType(typeof(List<CalculationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CalculationDto>>> List([FromQuery] int take = 20, CancellationToken ct = default)
    {
        take = Math.Clamp(take, 1, 200);

        var list = await _db.Calculations
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Take(take)
            .Select(e => ToDto(e))
            .ToListAsync(ct);

        return Ok(list);
    }

    // -------- Helpers --------

    private static (bool Ok, double K, double C, string? Error) NormalizeKc(SaveCalculationRequest req, double pmax)
    {
        // FIX: castear a (double?)null para que los ternarios sean del mismo tipo
        double? k = req.K ?? (req.KPercent.HasValue ? req.KPercent.Value / 100.0 : (double?)null);

        double? c = req.C
                    ?? (req.CPercent.HasValue ? req.CPercent.Value / 100.0 : (double?)null)
                    ?? (req.PMinW.HasValue ? Math.Clamp(req.PMinW.Value / pmax, 0.0, 1.0) : (double?)null);

        if (!k.HasValue) return (false, 0, 0, "Debes enviar 'k' (0–1) o 'kPercent' (0–100).");
        if (k.Value <= 0 || k.Value >= 1) return (false, 0, 0, "k debe estar en (0,1).");

        if (!c.HasValue) return (false, 0, 0, "Debes enviar 'c' (0–1), 'cPercent' (0–100) o 'pMinW' (W).");
        if (c.Value <= 0 || c.Value > 1) return (false, 0, 0, "c debe estar en (0,1].");

        return (true, k.Value, c.Value, null);
    }

    private static OperationRegime GuessRegime(double rth, double? rlStar)
    {
        if (!rlStar.HasValue) return OperationRegime.Tradeoff;
        var ratio = rlStar.Value / rth;
        if (ratio < 1.5) return OperationRegime.MaxPower;
        if (ratio < 3.5) return OperationRegime.Tradeoff;
        return OperationRegime.HighEfficiency;
    }

    private static CalculationDto ToDto(Calculation e) =>
        new(
            Id:            e.Id,
            Vth:           e.Vth,
            Rth:           e.Rth,
            K:             e.K,
            C:             e.C,
            Pmax:          e.Pmax,
            RlMin:         e.RlMin,
            RlMax:         e.RlMax,
            RecommendedRl: e.RecommendedRl,
            EtaAtRec:      e.EtaAtRec,
            PAtRec:        e.PAtRec,
            PMin:          e.PMin,
            PMaxByK:       e.PMaxByK,
            Regime:        e.Regime.ToString(),
            CreatedAtUtc:  e.CreatedAtUtc
        );
}
