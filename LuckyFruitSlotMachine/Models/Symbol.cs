namespace FruitSlot.Models;

public enum Symbol
{
    Cherry = 0,
    Lemon  = 1,
    Orange = 2,
    Plum   = 3,
    Bar    = 4,
    Seven  = 5
}

public static class SymbolExtensions
{
    public static string ToEmoji(this Symbol s) => s switch
    {
        Symbol.Cherry => "🍒",
        Symbol.Lemon  => "🍋",
        Symbol.Orange => "🍊",
        Symbol.Plum   => "🍇",
        Symbol.Bar    => "BAR",
        Symbol.Seven  => "7",
        _             => "?"
    };

    public static bool IsTextSymbol(this Symbol s) =>
        s == Symbol.Bar || s == Symbol.Seven;
}
