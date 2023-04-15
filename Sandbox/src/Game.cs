using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith;
using Monolith.assets;
using Monolith.graphics;
using Monolith.settings;
using Monolith.window;

namespace Sandbox;

public class Game : MMonolithGame
{
	private MMonolithWindow window;
	
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

		MStaticSprite sprite1 = new MStaticSprite(MTextureHelper.ScaleTexture(MAssetManager.GetTexture("player"), 4f, GraphicsDevice))
		{
			Position = new Vector2(500, 500),
			Centered = true
		};
		MStaticSprite sprite2 = new MStaticSprite(MTextureHelper.ScaleTexture(MAssetManager.GetTexture("player"), 4f, GraphicsDevice))
		{
			Position = new Vector2(500, 500),
			Scale = new Vector2(2f, 2f),
			Centered = true,
			Rotation = 1f
		};
		button = new MButton(sprite1, sprite2);
		
		base.LoadContent();
	}

	protected override void UnloadContent()
	{
		base.UnloadContent();
	}

	protected override void Update(GameTime gameTime)
	{
		button.Update();
		
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.Black);
		
		spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

		MStaticSprite sprite = new MStaticSprite(MAssetManager.GetTexture("player"))
		{
			Position = new Vector2(0, 0),
			Rotation = 0f,
			Centered = false
		};
		//sprite.Render(gameTime, spriteBatch);

		
		button.Render(gameTime, spriteBatch);
		
		spriteBatch.End();
		
		base.Draw(gameTime);
	}
}