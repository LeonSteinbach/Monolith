﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.scene;

namespace Monolith.graphics;

public class MStaticSprite : MSprite
{
	private readonly Texture2D texture;
	public override bool Centered { get; set; } = true;
	public override Color Color { get; set; } = Color.White;
	public override float Layer { get; set; }

	public MStaticSprite(Texture2D texture)
	{
		this.texture = texture;
		Position = Vector2.Zero;
	}
        
	public override Rectangle Hitbox
	{
		get
		{
			int width = (int)(texture.Width * Scale.X);
			int height = (int)(texture.Height * Scale.Y);
			int x = (int)(Position.X - Origin.X * Scale.X);
			int y = (int)(Position.Y - Origin.Y * Scale.Y);

			return new Rectangle(x, y, width, height);
		}
	}

	public override Vector2 Origin => Centered ? texture.Bounds.Size.ToVector2() / 2 : Vector2.Zero;

	public override void Update(GameTime gameTime) { }

	public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		spriteBatch.Draw(texture, Position, null, Color, Rotation, Origin, Scale, SpriteEffects.None, Layer);
	}

	public override void OnAddToNode(MNode parent) { }

	public override void OnRemoveFromNode(MNode parent) { }

	public override void OnTransformPosition() { }

	public override void OnTransformRotation() { }

	public override void OnTransformScale() { }
}