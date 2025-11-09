using System.ComponentModel.DataAnnotations;

namespace PnStudioAPI.Models;

public enum OperationRegime
{
    MaxPower = 0,
    Tradeoff = 1,
    HighEfficiency = 2
}

public class Calculation
{
    public long Id { get; set; }

    // 🔁 Ya NO es obligatorio. Sin [Required] y como int?
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }

    // Parámetros de entrada
    [Range(1e-9, double.MaxValue)] public double Vth { get; set; }
    [Range(1e-9, double.MaxValue)] public double Rth { get; set; }
    [Range(1e-9, 0.999999)]       public double K   { get; set; } // η mínima
    [Range(1e-9, 1.0)]            public double C   { get; set; } // P/Pmax mínima

    // Resultados derivados
    public double  Pmax  { get; set; }
    public double  RlMin { get; set; }
    public double  RlMax { get; set; }

    // ✅ Pueden ser nulos
    public double? RecommendedRl { get; set; }
    public double? EtaAtRec      { get; set; }
    public double? PAtRec        { get; set; }

    public double PMin    { get; set; }
    public double PMaxByK { get; set; }

    public OperationRegime Regime { get; set; } = OperationRegime.Tradeoff;
    public DateTime CreatedAtUtc  { get; set; } = DateTime.UtcNow;
}