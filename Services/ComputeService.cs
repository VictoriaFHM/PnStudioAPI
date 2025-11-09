using PnStudioAPI.Dto;

namespace PnStudioAPI.Services;

public class ComputeService
{
    public ComputeResponse Compute(ComputeRequest q)
    {
        // Pmax primero (lo usamos si llega PMinW)
        var pmax = CoreMath.PMax(q.Vth, q.Rth);

        // Normalizar k (0..1)
        double k = q.K ?? (q.KPercent.HasValue ? q.KPercent.Value / 100.0 : double.NaN);
        if (double.IsNaN(k) || k <= 0 || k >= 1)
            throw new ArgumentException("Debes enviar k (0..1) o kPercent (0..100).");

        // Normalizar c (0..1)
        double c;
        if (q.C.HasValue || q.CPercent.HasValue)
        {
            c = q.C ?? (q.CPercent!.Value / 100.0);
        }
        else if (q.PMinW.HasValue)
        {
            // Convertimos potencia absoluta a fracción de Pmax
            c = pmax <= 0 ? double.NaN : (q.PMinW.Value / pmax);
        }
        else
        {
            throw new ArgumentException("Debes enviar c/cPercent o pMinW.");
        }
        if (double.IsNaN(c)) throw new ArgumentException("No se pudo calcular 'c' a partir de pMinW.");
        c = Math.Clamp(c, 1e-9, 1.0);

        // Derivados
        var rlMin  = CoreMath.RlMinFromK(q.Rth, k);
        var rlMax  = CoreMath.RlMaxFromC(q.Rth, c);
        var etaMax = CoreMath.EtaMaxFromC(c);
        var pMin   = c * pmax;
        var pMaxK  = CoreMath.CMaxFromK(k) * pmax;

        // Factibilidad
        var feasible = (rlMin <= rlMax) && (k <= etaMax);

        double? rlStar = null, etaStar = null, pStar = null;
        if (feasible)
        {
            var tradeoff = 2.0 * q.Rth;                 // punto compromiso
            var rl = CoreMath.Clamp(tradeoff, rlMin, rlMax);

            rlStar  = rl;
            etaStar = CoreMath.EtaOfRL(q.Rth, rl);
            pStar   = CoreMath.POfRL(q.Vth, q.Rth, rl);
        }

        return new ComputeResponse(
            Feasible: feasible,
            Pmax: pmax,
            RlMin: rlMin,
            RlMax: rlMax,
            EtaMin: k,
            EtaMax: etaMax,
            PMin: pMin,
            PMaxByK: pMaxK,
            RecommendedRl: rlStar,
            EtaAtRec: etaStar,
            PAtRec: pStar
        );
    }
}
