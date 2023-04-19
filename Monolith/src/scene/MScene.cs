using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.scene;

namespace Monolith.scene;

public class MScene : MNode
{
	public override Rectangle Hitbox { get; }
	public override void Update(GameTime gameTime)
	{
		UpdateChildren(gameTime);
	}

	public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		if (!IsVisible) return;
		RenderChildren(graphics, spriteBatch, gameTime);
	}

	public override void OnAddToNode(MNode parent) { }

	public override void OnRemoveFromNode(MNode parent) { }
}