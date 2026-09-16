using Valour.Engine;

namespace Valour.Game;

internal class Program
{
    private static void Main()
    {
        using ValourEngine valourEngine = new ValourEngine("ValourGame", 800, 800, false, true);
        valourEngine.Run(new ValourGame());
    }
}