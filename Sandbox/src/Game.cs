using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith;
using Monolith.assets;
using Monolith.graphics;
using Monolith.scene;
using Monolith.settings;
using Monolith.window;

namespace Sandbox;

public class Game : MMonolithGame
{
	private MMonolithWindow window;

	private MScene mainScene;

	public Game()
	{
		window = new MMonolithWindow(this)
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
		MAppSettings.Content = Content;
		MAppSettings.ContentRoot = "res";

		base.Initialize();
	}

	protected override void LoadContent()
	{
		MAssetManager.LoadTexture("player", @"images\player");

		mainScene = new MScene();
		MScene scene1 = new MScene();
		MScene scene2 = new MScene();
		
		MNode n1 = new MNode(new Vector2(100, 100), Vector2.One, 0f, "n1");
		MNode n2 = new MNode(new Vector2(200, 200), Vector2.One, 0f, "n2");
		MNode n3 = new MNode(new Vector2(300, 300), Vector2.One, 0f, "n3");
		
		scene1.AddNode(n1);
		scene1.AddNode(n2);
		scene2.AddNode(n3);
		
		mainScene.AddScene(scene1);
		mainScene.AddScene(scene2);

		base.LoadContent();
	}

	protected override void UnloadContent()
	{
		base.UnloadContent();
	}

	protected override void Update(GameTime gameTime)
	{
		mainScene.Update(gameTime);
		
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.Black);
		
		spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

		mainScene.Render(graphics, spriteBatch, gameTime);
		
		spriteBatch.End();
		
		base.Draw(gameTime);
	}
}