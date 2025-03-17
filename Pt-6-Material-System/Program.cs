namespace OpenTK_Game;

internal abstract class Program
{
    private static void Main()
    {
        using var game = new Game();
        game.Run();
    }
}