namespace PnStudioAPI.Services;

public static class CoreMath
{
    // Pmax = Vth^2 / (4 Rth)
    public static double PMax(double vth, double rth) => (vth * vth) / (4.0 * rth);

    // RL_min(k) = (k/(1-k)) * Rth
    public static double RlMinFromK(double rth, double k) => (k / (1.0 - k)) * rth;

    // RL_max(c) = Rth * ( [ 2(1 + sqrt(1-c)) - c ] / c )
    public static double RlMaxFromC(double rth, double c)
        => rth * ((2.0 * (1.0 + Math.Sqrt(1.0 - c)) - c) / c);

    // η_max(c) = (1 + sqrt(1-c)) / 2
    public static double EtaMaxFromC(double c) => (1.0 + Math.Sqrt(1.0 - c)) / 2.0;

    // c_max(k) = 4 k (1-k)
    public static double CMaxFromK(double k) => 4.0 * k * (1.0 - k);

    // P(RL) = V^2 * RL / (Rth + RL)^2
    public static double POfRL(double vth, double rth, double rl)
        => (vth * vth * rl) / Math.Pow(rth + rl, 2.0);

    // η(RL) = RL / (Rth + RL)
    public static double EtaOfRL(double rth, double rl) => rl / (rth + rl);

    // Utilidad
    public static double Clamp(double x, double lo, double hi) => Math.Min(Math.Max(x, lo), hi);
}