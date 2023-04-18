using Microsoft.Xna.Framework;
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

	public override Rectangle Hitbox => sprite.Hitbox;

	public override void Update(GameTime gameTime)
	{
		Position += Vector2.One;
		
		UpdateChildren(gameTime);
	}

	public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		if (IsVisible)
			sprite.Render(graphics, spriteBatch, gameTime);
		
		RenderChildren(graphics, spriteBatch, gameTime);
	}

	public override void OnAddToNode(MNode parent)
	{
		
	}

	public override void OnRemoveFromNode(MNode parent)
	{
		
	}

	public override void OnTransformPosition()
	{
		sprite.Position = Position;
	}
	
	public override void OnTransformRotation()
	{
		sprite.Rotation = Rotation;
	}
	
	public override void OnTransformScale()
	{
		sprite.Scale = Scale;
	}
}