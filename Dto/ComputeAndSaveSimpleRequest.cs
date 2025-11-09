using System.ComponentModel.DataAnnotations;

namespace PnStudioAPI.Dto;

public class ComputeAndSaveSimpleRequest
{
    [Required] public double Vth { get; set; }
    [Required] public double Rth { get; set; }

    // Provee UNO para eficiencia (k en [0..1] o kPercent en [0..100])
    public double? K { get; set; }
    public double? KPercent { get; set; }

    // Provee UNO para potencia mínima (c en [0..1], cPercent en [0..100] o pMinW absoluta)
    public double? C { get; set; }
    public double? CPercent { get; set; }
    public double? PMinW { get; set; }
}