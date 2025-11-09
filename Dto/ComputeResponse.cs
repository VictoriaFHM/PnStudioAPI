namespace PnStudioAPI.Dto;

/// <summary>
/// Resumen del cálculo de banda factible y recomendación.
/// </summary>
public sealed record ComputeResponse(
    bool   Feasible,   // ¿Existe intersección entre restricciones?
    double Pmax,       // Vth^2/(4*Rth)
    double RlMin,      // R_L^min(k) = (k/(1-k))*Rth
    double RlMax,      // R_L^max(c) = Rth * ((2*(1+sqrt(1-c)) - c)/c)
    double EtaMin,     // k solicitado
    double EtaMax,     // η_max(c) = (1 + sqrt(1-c))/2
    double PMin,       // c * Pmax (en W)
    double PMaxByK,    // 4k(1-k) * Pmax (en W)
    double? RecommendedRl, // RL* (recomendación) - puede ser null si no es factible
    double? EtaAtRec,      // η(RL*) - puede ser null
    double? PAtRec         // P(RL*) en W - puede ser null
);