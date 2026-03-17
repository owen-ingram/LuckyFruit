using FruitSlot.Models;

namespace FruitSlot.Models;

public class ReelStrip
{
    private readonly Symbol[] _strip;

    public string Name   { get; }
    public int    Length => _strip.Length;

    public ReelStrip(string name, Symbol[] strip)
    {
        Name   = name;
        _strip = strip;
    }

    public Symbol GetSymbol(int stop) => _strip[stop % Length];
    
    public Dictionary<Symbol, int> GetCounts()
    {
        var counts = Enum.GetValues<Symbol>().ToDictionary(s => s, _ => 0);
        foreach (var s in _strip) counts[s]++;
        return counts;
    }
}
