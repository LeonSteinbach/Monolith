﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith;
using Monolith.assets;
using Monolith.graphics;
using Monolith.particles;
using Monolith.settings;
using Monolith.window;

namespace Sandbox;

public class Game : MMonolithGame
{
	private MMonolithWindow window;

	private MScene mainScene, guiScene, playerScene;

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
		guiScene = new MScene();
		playerScene = new MScene();

		var s1 = new MStaticSprite(player)
		{
			Position = new Vector2(200, 200),
			Color = Color.Red
		};
		
		var s2 = new MStaticSprite(player)
		{
			Position = new Vector2(200, 200),
			Color = Color.Green
		};
		
		var s3 = new MStaticSprite(player)
		{
			Position = new Vector2(200, 200),
			Color = Color.Blue
		};
		
		var s4 = new MStaticSprite(player)
		{
			Position = new Vector2(200, 200),
			Color = Color.Yellow
		};
		
		var s5 = new MStaticSprite(player)
		{
			Position = new Vector2(200, 400),
			Color = Color.Red
		};
		
		var s6 = new MStaticSprite(player)
		{
			Position = new Vector2(200, 400),
			Color = Color.Green
		};
		
		var s7 = new MStaticSprite(player)
		{
			Position = new Vector2(200, 400),
			Color = Color.Blue
		};
		
		var s8 = new MStaticSprite(player)
		{
			Position = new Vector2(200, 400),
			Color = Color.Yellow
		};
		
		var s9 = new MStaticSprite(player)
		{
			Position = new Vector2(200, 600),
			Color = Color.Red
		};
		
		var s10 = new MStaticSprite(player)
		{
			Position = new Vector2(200, 600),
			Color = Color.Green
		};
		
		var s11 = new MStaticSprite(player)
		{
			Position = new Vector2(200, 600),
			Color = Color.Blue
		};
		
		var s12 = new MStaticSprite(player)
		{
			Position = new Vector2(200, 600),
			Color = Color.Yellow
		};
		
		var rb1 = new MRadioButton(s1, s2, s3, s4, isChecked: false)
		{
			Name = "radio1"
		};
		
		var rb2 = new MRadioButton(s5, s6, s7, s8, isChecked: true)
		{
			Name = "radio2"
		};
		
		var rb3 = new MRadioButton(s9, s10, s11, s12, isChecked: false)
		{
			Name = "radio3"
		};

		var rg1 = new MRadioGroup();
		rg1.AddButton(rb1);
		rg1.AddButton(rb2);
		rg1.AddButton(rb3);

		var s13 = new MStaticSprite(player)
		{
			Position = new Vector2(500, 200),
			Scale = new Vector2(5, 5),
			Color = Color.Wheat
		};

		var s14 = new MStaticSprite(player)
		{
			Color = Color.Yellow
		};
		
		var s15 = new MStaticSprite(player)
		{
			Color = Color.Magenta
		};
		
		var t1 = new MText("button", MAssetManager.GetFont("arial"))
		{
			Position = s13.Position,
			Color = Color.Yellow
		};

		var b1 = new MButton(s14, s15, text: t1);

		var sl1 = new MSlider(s13, b1, -5, 5, 0, MSliderDirection.Vertical);

		var p1 = new Player()
		{
			Position = new Vector2(100, 100)
		};

		var t2 = new MText("hallo", arial)
		{
			Position = new Vector2(700, 200),
			Color = Color.Red,
			Scale = new Vector2(3, 2)
		};

		var tbText = new MText("", arial)
		{
			Position = new Vector2(800, 300),
			Color = Color.White,
			Scale = new Vector2(5, 1)
		};
		
		var tbPlaceholder = new MText("Your name", arial)
		{
			Position = new Vector2(800, 300),
			Color = Color.Gray 
		};

		var tbBackground = new MStaticSprite(player)
		{
			Position = new Vector2(800, 300),
			Scale = new Vector2(3)
		};

		var tbCursor = new MAnimatedSprite(cursor, 2, 1, 2, 500);

		var tb1 = new MTextbox(tbBackground, tbText, tbCursor, tbPlaceholder, 10, true);

		var pbBackground = new MStaticSprite(player)
		{
			Position = new Vector2(300, 100),
			Scale = new Vector2(4, 1)
		};

		var pbForeground = new MStaticSprite(player)
		{
			Position = pbBackground.Position,
			Scale = new Vector2(4f, 1f),
			Color = Color.Yellow
		};

		var pbText = new MText("test", arial)
		{
			Position = pbBackground.Position
		};

		var pb1 = new MProgressBar(pbBackground, pbForeground, 0, 100, 70, pbText, MProgressBarDirection.Horizontal);
		
		guiScene.AddNode(pb1);
		guiScene.AddNode(sl1);
		guiScene.AddNode(rg1);
		guiScene.AddNode(t2);
		guiScene.AddNode(tb1);
		
		playerScene.AddNode(p1);
		
		mainScene.AddNode(guiScene);
		mainScene.AddNode(playerScene);

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