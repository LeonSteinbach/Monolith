using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.assets;
using Monolith.graphics;
using Monolith.math;
using Monolith.physics;
using Monolith.scene;

namespace Sandbox;

public class Player : MBody
{
	public readonly MStaticSprite sprite;
	
	public Player()
	{
		sprite = new MStaticSprite(MAssetManager.GetTexture("player"))
		{
			Position = Position,
			Rotation = Rotation,
			Scale = Scale
		};
	}

	public override MGeometryObject Hitbox => sprite.Hitbox;

	public override void Update(GameTime gameTime)
	{
		UpdateChildren(gameTime);
		
		base.Update(gameTime);
	}

	public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		if (!IsVisible) return;
		sprite.Render(graphics, spriteBatch, gameTime);
		RenderChildren(graphics, spriteBatch, gameTime);
		
		base.Render(graphics, spriteBatch, gameTime);
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
		base.OnTransformPosition();
	}
	
	public override void OnTransformRotation()
	{
		sprite.Rotation = Rotation;
		base.OnTransformRotation();
	}
	
	public override void OnTransformScale()
	{
		sprite.Scale = Scale;
		base.OnTransformScale();
	}
}