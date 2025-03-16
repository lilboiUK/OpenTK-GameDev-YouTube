namespace OpenTK_Game;

internal abstract class Program
{
    static void Main()
    {
        using var game = new Game();
        game.Run();
    }
}