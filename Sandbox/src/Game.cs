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

	private MRadioGroup radioGroup;
	private MRadioButton button1, button2;
	
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
			Position = new Vector2(500, 200)
		};
		MStaticSprite sprite2 = new MStaticSprite(MTextureHelper.ScaleTexture(MAssetManager.GetTexture("player"), 4f, GraphicsDevice))
		{
			Position = new Vector2(500, 200),
			Scale = new Vector2(2f, 2f)
		};
		MStaticSprite sprite3 = new MStaticSprite(MTextureHelper.ScaleTexture(MAssetManager.GetTexture("player"), 4f, GraphicsDevice))
		{
			Position = new Vector2(500, 200),
			Scale = new Vector2(3f, 3f)
		};
		MStaticSprite sprite4 = new MStaticSprite(MTextureHelper.ScaleTexture(MAssetManager.GetTexture("player"), 4f, GraphicsDevice))
		{
			Position = new Vector2(500, 200),
			Scale = new Vector2(4f, 4f)
		};
		
		MStaticSprite sprite5 = new MStaticSprite(MTextureHelper.ScaleTexture(MAssetManager.GetTexture("player"), 4f, GraphicsDevice))
		{
			Position = new Vector2(500, 600)
		};
		MStaticSprite sprite6 = new MStaticSprite(MTextureHelper.ScaleTexture(MAssetManager.GetTexture("player"), 4f, GraphicsDevice))
		{
			Position = new Vector2(500, 600),
			Scale = new Vector2(2f, 2f)
		};
		MStaticSprite sprite7 = new MStaticSprite(MTextureHelper.ScaleTexture(MAssetManager.GetTexture("player"), 4f, GraphicsDevice))
		{
			Position = new Vector2(500, 600),
			Scale = new Vector2(3f, 3f)
		};
		MStaticSprite sprite8 = new MStaticSprite(MTextureHelper.ScaleTexture(MAssetManager.GetTexture("player"), 4f, GraphicsDevice))
		{
			Position = new Vector2(500, 600),
			Scale = new Vector2(4f, 4f)
		};
		
		button1 = new MRadioButton(sprite1, sprite2, sprite3, sprite4, text: "hallo", isChecked: true);
		button2 = new MRadioButton(sprite5, sprite6, sprite7, sprite8, text: "asdf", isChecked: false);

		radioGroup = new MRadioGroup();
		radioGroup.AddButton(button1);
		radioGroup.AddButton(button2);
		
		base.LoadContent();
	}

	protected override void UnloadContent()
	{
		base.UnloadContent();
	}

	protected override void Update(GameTime gameTime)
	{
		radioGroup.Update();
		Console.WriteLine(radioGroup.GetSelectedText());
		
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.Black);
		
		spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

		radioGroup.Render(gameTime, spriteBatch);
		
		spriteBatch.End();
		
		base.Draw(gameTime);
	}
}