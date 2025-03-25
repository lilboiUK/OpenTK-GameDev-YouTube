namespace OpenTK_Game;

internal abstract class Program
{
    private static void Main()
    {
        using var game = new Game(1000, 1000, "Chill Code Sessions 3D");
        game.Run();
    }
}