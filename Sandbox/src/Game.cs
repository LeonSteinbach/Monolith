using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith;
using Monolith.assets;
using Monolith.input;
using Monolith.math;
using Monolith.physics;
using Monolith.scene;
using Monolith.settings;
using Monolith.window;

namespace Sandbox;

public class Game : MMonolithGame
{
	private MMonolithWindow window;

	private MScene mainScene;

	private Player player1, player2;
	private Platform platform1, platform2, platform3;

	private MWorld world;

	private MPolygon polygon;
	private MPolygon rectangle;
	private MCircle circle;

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

		world = new MWorld();

		player1 = new Player()
		{
			Position = new Vector2(400, 200),
			Scale = new Vector2(4f),
			Inertia = 0.5f,
			IsVisible = true,
			Rotation = 0.01f
		};
		
		player2 = new Player()
		{
			Position = new Vector2(450, 550),
			Scale = new Vector2(4f),
			Inertia = 0.5f,
			IsVisible = true,
			Rotation = 0.1f,
			IsStatic = true
		};

		platform1 = new Platform(new []
		{
			new Vector2(0, 750),
			new Vector2(1100, 750),
			new Vector2(1100, 800),
			new Vector2(0, 800),
		});
		
		platform2 = new Platform(new []
		{
			new Vector2(0, 0),
			new Vector2(50, 0),
			new Vector2(50, 800),
			new Vector2(0, 800)
		});
		
		platform3 = new Platform(new []
		{
			new Vector2(300, 300),
			new Vector2(500, 350),
			new Vector2(480, 390),
			new Vector2(280, 330)
		});
		
		world.AddBody(player1);
		world.AddBody(player2);

		for (int i = 0; i < 0; i++)
		{
			int x = MMathHelper.RandInt(100, window.Size.X - 100);
			int y = MMathHelper.RandInt(100, window.Size.Y - 100);
			
			var body = new Player()
			{
				Position = new Vector2(x, y),
				Inertia = 0.1f,
				IsStatic = MMathHelper.RandFloat() > 0.8
			};

			var dir = new Vector2(MMathHelper.RandFloat() * 2 - 1, MMathHelper.RandFloat() * 2 - 1);
			dir.Normalize();
			dir *= 30f;
			body.LinearVelocity = dir; 
			world.AddBody(body);
		}
		
		world.AddBody(platform1);
		world.AddBody(platform2);
		world.AddBody(platform3);
		
		//player2.LinearVelocity = new Vector2(-0, -20);

		base.LoadContent();
	}

	protected override void UnloadContent()
	{
		base.UnloadContent();
	}

	protected override void Update(GameTime gameTime)
	{
		mainScene.Update(gameTime);

		if (MInput.IsLeftPressed())
		{
			var vel = MInput.MousePosition().ToVector2() - player1.Position;
			vel.Normalize();
			vel *= 500f;
			player1.LinearVelocity = vel;
		}

		world.Update(gameTime);

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.Black);
		
		spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
		
		mainScene.Render(graphics, spriteBatch, gameTime);
		
		world.Render(graphics, spriteBatch, gameTime);
		
		spriteBatch.End();
		
		base.Draw(gameTime);
	}
}