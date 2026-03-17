using FruitSlot.Models;

namespace FruitSlot.Models;

public class PayTable
{
    private readonly Dictionary<Symbol, int> _prizes = new()
    {
        [Symbol.Cherry] = 10,
        [Symbol.Lemon]  = 15,
        [Symbol.Orange] = 20,
        [Symbol.Plum]   = 30,
        [Symbol.Bar]    = 100,
        [Symbol.Seven]  = 500,
    };

    public int GetPrize(Symbol symbol) =>
        _prizes.TryGetValue(symbol, out int prize) ? prize : 0;

    public IEnumerable<(Symbol Symbol, int Prize)> AllPrizes =>
        _prizes.Select(kv => (kv.Key, kv.Value)).OrderBy(x => x.Key);
}
