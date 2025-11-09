using System.ComponentModel.DataAnnotations;

namespace PnStudioAPI.Dto;

public class SaveCalculationRequest
{
    [Required, Range(1e-9, double.MaxValue)]
    public double Vth { get; set; }

    [Required, Range(1e-9, double.MaxValue)]
    public double Rth { get; set; }

    // Formas equivalentes de enviar la eficiencia
    [Range(1e-9, 0.999999)] public double? K { get; set; }
    [Range(1, 100)]         public double? KPercent { get; set; }

    // Formas equivalentes de enviar potencia relativa mínima
    [Range(1e-9, 1.0)]      public double? C { get; set; }
    [Range(1, 100)]         public double? CPercent { get; set; }
    [Range(0, double.MaxValue)]
    public double? PMinW { get; set; } // alternativa: potencia mínima en Watts

    // opcional: si estás usando proyectos
    public int? ProjectId { get; set; }
}