using FruitSlot.Models;

namespace FruitSlot.Engine;

public class SpinEngine
{
    private readonly ReelStrip[] _reels;
    private readonly PayTable    _payTable;
    private readonly Random      _rng = new();

    public SpinEngine(ReelStrip[] reels, PayTable payTable)
    {
        _reels    = reels;
        _payTable = payTable;
    }
    
    public SpinResult Spin()
    {
        int[] stops = _reels.Select(r => _rng.Next(r.Length)).ToArray();

        Symbol[] symbols = stops.Select((stop, i) => _reels[i].GetSymbol(stop)).ToArray();

        bool isWin = symbols[0] == symbols[1] && symbols[1] == symbols[2];
        int  prize = isWin ? _payTable.GetPrize(symbols[0]) : 0;

        return new SpinResult(stops, symbols, prize);
    }
}

public record SpinResult(
    int[]    Stops,
    Symbol[] Symbols,
    int      Prize
)
{
    public bool IsWin => Prize > 0;
}
