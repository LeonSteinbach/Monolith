using Monolith;
using Microsoft.Xna.Framework;

namespace Sandbox;

public class Game : MonolithGame
{
	private MonolithWindow window;
	
	public Game()
	{
		window = new MonolithWindow(this)
		{
			Size = new Point(1200, 800),
			Title = "Sandbox",
			IsCentered = true,
			IsFullScreen = false,
			IsMouseVisible = true,
			IsFixedTimeStep = true,
			IsVSyncEnabled = false
		};
	}

	protected override void Initialize()
	{
		AppSettings.Content = Content;
		AppSettings.ContentRoot = "res";
		
		base.Initialize();
	}

	protected override void LoadContent()
	{
		base.LoadContent();
	}

	protected override void UnloadContent()
	{
		base.UnloadContent();
	}

	protected override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		base.Draw(gameTime);
	}
}