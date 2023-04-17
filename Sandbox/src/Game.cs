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

	private MScene mainScene, scene1, scene2;

	private MButton button;

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
			IsVSyncEnabled = true,
			IsResizeAllowed = false,
			IsBorderless = false
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
		scene1 = new MScene();
		scene2 = new MScene();
		
		Player n1 = new Player(){Position = new Vector2(100, 100), Name = "p1"};
		Player n2 = new Player(){Position = new Vector2(200, 200), Name = "p2"};
		Player n3 = new Player(){Position = new Vector2(300, 300), Name = "p3"};

		MStaticSprite s1 = new MStaticSprite(MAssetManager.GetTexture("player"))
		{
			Position = new Vector2(100, 100)
		};
		MStaticSprite s2 = new MStaticSprite(MAssetManager.GetTexture("player"))
		{
			Position = new Vector2(100, 100),
			Scale = new Vector2(2, 2)
		};
		button = new MButton(s1, s2);
		
		scene1.AddNode(button);
		
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
		scene1.Update(gameTime);
		
		button.Update(gameTime);
		
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.Black);
		
		spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

		mainScene.Render(graphics, spriteBatch, gameTime);
		scene1.Render(graphics, spriteBatch, gameTime);

		spriteBatch.End();
		
		base.Draw(gameTime);
	}
}