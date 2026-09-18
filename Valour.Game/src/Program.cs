using Valour.Engine;

namespace Valour.Game;

internal class Program
{
    private static void Main()
    {
        using ValourEngine valourEngine = new ValourEngine("ValourGame", 1200, 1200, false, true);
        valourEngine.Run(new ValourGame());
    }
}