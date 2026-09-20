using Valour.Engine;

namespace Valour.Game;

internal class Program
{
    private static void Main()
    {
        using ValourEngine valourEngine = new ValourEngine("ValourGame", 1920, 1080, false, false);
        valourEngine.Run(new ValourGame());
    }
}