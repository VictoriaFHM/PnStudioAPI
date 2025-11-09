namespace PnStudioAPI.Dto;

public sealed record CalculationDto(
    long   Id,
    double Vth,
    double Rth,
    double K,
    double C,
    double Pmax,
    double RlMin,
    double RlMax,
    double? RecommendedRl,
    double? EtaAtRec,
    double? PAtRec,
    double PMin,
    double PMaxByK,
    string Regime,
    DateTime CreatedAtUtc
);