using FruitSlot.Engine;
using FruitSlot.Models;

namespace FruitSlot.Reports;

public static class ParSheetGenerator
{
    public static void Write(string path, ReelStrip[] reels, PayTable payTable)
    {
        var engine  = new MathEngine(reels, payTable);
        var entries = engine.GetPrizeDistribution();
        double rtp  = engine.GetRTP();
        double hitFreq = engine.GetHitFrequency();

        using var w = new StreamWriter(path);

        w.WriteLine("PAR SHEET — Lucky Fruits");
        w.WriteLine($"Generated,{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
        w.WriteLine();

        w.WriteLine("GAME CONFIGURATION");
        w.WriteLine("Parameter,Value");
        w.WriteLine("Reels,3");
        w.WriteLine("Rows,1 (centre payline only)");
        w.WriteLine("Paylines,1");
        w.WriteLine("Win condition,3-of-a-kind left to right");
        w.WriteLine($"Stops per reel,{reels[0].Length}");
        w.WriteLine();

        w.WriteLine("REEL STRIP SYMBOL COUNTS");
        w.Write("Symbol");
        for (int i = 0; i < reels.Length; i++) w.Write($",{reels[i].Name}");
        w.WriteLine(",Prize (×bet)");

        foreach (var (sym, prize) in payTable.AllPrizes)
        {
            w.Write(sym);
            foreach (var reel in reels)
                w.Write($",{reel.GetCounts()[sym]}");
            w.WriteLine($",{prize}");
        }
        w.Write("TOTAL");
        foreach (var reel in reels) w.Write($",{reel.Length}");
        w.WriteLine();
        w.WriteLine();

        w.WriteLine("PRIZE DISTRIBUTION");
        w.WriteLine("Symbol,Prize,P(Reel1),P(Reel2),P(Reel3),P(3-of-3),1-in-N,Expected Value,% of RTP");

        foreach (var e in entries)
        {
            string oneInN = double.IsInfinity(e.OneInN) ? "Never" : e.OneInN.ToString("F0");
            w.WriteLine(string.Join(",",
                e.Symbol,
                e.Prize,
                e.ProbReel1.ToString("F5"),
                e.ProbReel2.ToString("F5"),
                e.ProbReel3.ToString("F5"),
                e.Probability3of3.ToString("G6"),
                oneInN,
                e.ExpectedValue.ToString("G6"),
                (e.RtpContribution(rtp) * 100).ToString("F2") + "%"
            ));
        }
        w.WriteLine();

        w.WriteLine("SUMMARY");
        w.WriteLine("Metric,Value");
        w.WriteLine($"Theoretical RTP,{rtp:P4}");
        w.WriteLine($"House Edge,{1 - rtp:P4}");
        w.WriteLine($"Hit Frequency,{hitFreq:P4}");
        w.WriteLine($"1-in-N spins wins,{1.0 / hitFreq:F1}");
    }
}
