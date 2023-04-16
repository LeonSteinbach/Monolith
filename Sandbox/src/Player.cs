using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.assets;
using Monolith.graphics;
using Monolith.scene;

namespace Sandbox;

public class Player : MNode
{
	private MStaticSprite sprite;
	
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
		
	}

	public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		
	}

	public override void OnAddToScene(MScene scene)
	{
		
	}

	public override void OnRemoveFromScene(MScene scene)
	{
		
	}

	protected override void OnTransformChanged()
	{
		
	}
}