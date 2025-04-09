namespace OpenTK_Game;

internal abstract class Program
{
    private static void Main()
    {
        using var game = new Game(1920, 1080, "Chill Code Sessions 3D");
        game.Run();
    }
}