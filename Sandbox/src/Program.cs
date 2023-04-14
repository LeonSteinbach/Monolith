namespace Sandbox;

public static class Program
{
	[STAThread]
	public static void Main(string[] args)
	{
		Game game = new Game();
		game.Run();
	}
}