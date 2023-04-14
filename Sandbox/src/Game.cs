using Microsoft.Xna.Framework;
using Monolith;
using Monolith.assets;
using Monolith.settings;
using Monolith.window;

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
		AssetManager.LoadTexture("player", @"images\player");
		
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
		GraphicsDevice.Clear(Color.Black);
		
		spriteBatch.Begin();
		
		spriteBatch.Draw(AssetManager.GetTexture("player"), new Vector2(100, 200), Color.White);
		
		spriteBatch.End();
		
		base.Draw(gameTime);
	}
}