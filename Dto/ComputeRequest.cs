using System.ComponentModel.DataAnnotations;

namespace PnStudioAPI.Dto;

public class ComputeRequest
{
    // Thévenin
    [Required, Range(1e-9, double.MaxValue)] public double Vth { get; set; }
    [Required, Range(1e-9, double.MaxValue)] public double Rth { get; set; }

    // --- Eficiencia: envía UNO de los dos ---
    // Normalizado (0..1)  ej: 0.60
    [Range(1e-9, 0.999999)] public double? K { get; set; }
    // Porcentaje (0..100) ej: 60
    [Range(0.0001, 99.9999)] public double? KPercent { get; set; }

    // --- Potencia mínima: envía UNO de los tres ---
    // Relativo (0..1)      ej: 0.85
    [Range(1e-9, 1.0)] public double? C { get; set; }
    // Porcentaje (0..100)  ej: 85
    [Range(0.0001, 100.0)] public double? CPercent { get; set; }
    // Absoluto en watts    ej: 0.004
    [Range(0.0, double.MaxValue)] public double? PMinW { get; set; }
}