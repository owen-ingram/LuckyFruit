using FruitSlot.Models;

namespace FruitSlot.Data;

public static class ReelDefinitions
{
    private static readonly Symbol[] Strip = BuildStrip();

    public static ReelStrip[] GetReels() => new[]
    {
        new ReelStrip("Reel 1", Strip),
        new ReelStrip("Reel 2", Strip),
        new ReelStrip("Reel 3", Strip),
    };

    private static Symbol[] BuildStrip()
    {
        var strip = new List<Symbol>();
        for (int i = 0; i < 7; i++) strip.Add(Symbol.Cherry);
        for (int i = 0; i < 5; i++) strip.Add(Symbol.Lemon);
        for (int i = 0; i < 4; i++) strip.Add(Symbol.Orange);
        for (int i = 0; i < 2; i++) strip.Add(Symbol.Plum);
        strip.Add(Symbol.Bar);
        strip.Add(Symbol.Seven);
        Shuffle(strip);
        return strip.ToArray();
    }

    private static void Shuffle(List<Symbol> list)
    {
        var rng = new Random(42);
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
