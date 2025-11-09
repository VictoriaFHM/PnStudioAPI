using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using PnStudioAPI.Dto;
using PnStudioAPI.Services;

namespace PnStudioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ComputeController : ControllerBase
{
    private readonly ComputeService _svc;
    public ComputeController(ComputeService svc) => _svc = svc;

    /// <summary>
    /// Calcula banda factible de RL, recomendación RL* y métricas P/η.
    /// Acepta k ó kPercent, y c ó cPercent ó pMinW.
    /// </summary>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ComputeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public ActionResult<ComputeResponse> Post([FromBody] ComputeRequest req)
    {
        if (req.Vth <= 0 || req.Rth <= 0)
            return BadRequest(new { error = "Vth y Rth deben ser mayores que 0." });

        try
        {
            var res = _svc.Compute(req);
            return Ok(res);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Ejemplo rápido con valores por defecto (útil para probar desde el navegador).
    /// Podés enviar k/kPercent y c/cPercent/pMinW por querystring.
    /// </summary>
    [HttpGet("example")]
    [ProducesResponseType(typeof(ComputeResponse), StatusCodes.Status200OK)]
    public ActionResult<ComputeResponse> Example(
        [FromQuery] double vth = 5.0,
        [FromQuery] double rth = 1000.0,
        [FromQuery] double? k = 0.60,
        [FromQuery] double? kPercent = null,
        [FromQuery] double? c = 0.85,
        [FromQuery] double? cPercent = null,
        [FromQuery] double? pMinW = null
    )
    {
        var req = new ComputeRequest
        {
            Vth = vth,
            Rth = rth,
            K = k,
            KPercent = kPercent,
            C = c,
            CPercent = cPercent,
            PMinW = pMinW
        };

        var res = _svc.Compute(req);
        return Ok(res);
    }
}
