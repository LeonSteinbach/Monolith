﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.assets;
using Monolith.graphics;
using Monolith.scene;

namespace Sandbox;

public class Player : MNode
{
	private readonly MStaticSprite sprite;
	
	public Player()
	{
		sprite = new MStaticSprite(MAssetManager.GetTexture("player"))
		{
			Position = Position,
			Rotation = Rotation,
			Scale = Scale
		};
	}

	public override void Update(GameTime gameTime)
	{
		Console.WriteLine($"update node: ${Name}");
		Position += Vector2.One;
	}

	public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		if (IsVisible)
			sprite.Render(gameTime, spriteBatch);
	}

	public override void OnAddToScene(MScene scene)
	{
		
	}

	public override void OnRemoveFromScene(MScene scene)
	{
		
	}

	protected override void OnTransformChanged()
	{
		Console.WriteLine($"transform: ${Position}");
		sprite.Position = Position;
	}
}