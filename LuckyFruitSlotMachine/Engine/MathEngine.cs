using FruitSlot.Models;

namespace FruitSlot.Engine;

public class MathEngine
{
    private readonly ReelStrip[] _reels;
    private readonly PayTable    _payTable;

    public MathEngine(ReelStrip[] reels, PayTable payTable)
    {
        if (reels.Length != 3) throw new ArgumentException("Need exactly 3 reels.");
        _reels    = reels;
        _payTable = payTable;
    }

    public List<PrizeEntry> GetPrizeDistribution()
    {
        var entries = new List<PrizeEntry>();

        var probsPerReel = _reels
            .Select(r => r.GetCounts().ToDictionary(
                kv => kv.Key,
                kv => (double)kv.Value / r.Length))
            .ToArray();

        foreach (var (symbol, prize) in _payTable.AllPrizes)
        {
            double p1 = probsPerReel[0][symbol];
            double p2 = probsPerReel[1][symbol];
            double p3 = probsPerReel[2][symbol];

            double probability = p1 * p2 * p3;

            entries.Add(new PrizeEntry(
                Symbol:           symbol,
                Prize:            prize,
                ProbReel1:        p1,
                ProbReel2:        p2,
                ProbReel3:        p3,
                Probability3of3:  probability,
                ExpectedValue:    probability * prize
            ));
        }

        return entries;
    }

    public double GetRTP() => GetPrizeDistribution().Sum(e => e.ExpectedValue);

    public double GetHitFrequency() => GetPrizeDistribution().Sum(e => e.Probability3of3);
}

public record PrizeEntry(
    Symbol Symbol,
    int    Prize,
    double ProbReel1,
    double ProbReel2,
    double ProbReel3,
    double Probability3of3,
    double ExpectedValue
)
{
    public double OneInN => Probability3of3 > 0 ? 1.0 / Probability3of3 : double.PositiveInfinity;
    public double RtpContribution(double totalRtp) => totalRtp > 0 ? ExpectedValue / totalRtp : 0;
}
