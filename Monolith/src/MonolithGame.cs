﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monolith.assets;

namespace Monolith;

public class MonolithGame : Game
{
	public GraphicsDeviceManager graphics;
	protected SpriteBatch spriteBatch;

	protected MonolithGame()
	{
		graphics = new GraphicsDeviceManager(this);
	}

	protected override void Initialize()
	{
		AssetManager.Initialize();
		
		base.Initialize();
	}

	protected override void LoadContent()
	{
		spriteBatch = new SpriteBatch(GraphicsDevice);
	}

	protected override void Update(GameTime gameTime)
	{
		if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			Keyboard.GetState().IsKeyDown(Keys.Escape))
			Exit();

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		base.Draw(gameTime);
	}
}