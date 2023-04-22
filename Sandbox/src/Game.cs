using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith;
using Monolith.assets;
using Monolith.graphics;
using Monolith.math;
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
		MAssetManager.LoadTexture("cursor", @"images\cursor");
		MAssetManager.LoadFont("arial", @"fonts\arial");
		
		var player = MTextureHelper.ScaleTexture(MAssetManager.GetTexture("player"), 2f, GraphicsDevice);
		var cursor = MTextureHelper.ScaleTexture(MAssetManager.GetTexture("cursor"), 1f, GraphicsDevice);
		var arial = MAssetManager.GetFont("arial");

		mainScene = new MScene();
		
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