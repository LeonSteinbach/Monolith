using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.assets;
using Monolith.scene;

namespace Monolith.graphics;

public class MText : MSprite
{
	public string Text { get; set; }
	public SpriteFont Font { get; set; }
	public override bool Centered { get; set; } = true;
	public override Color Color { get; set; } = Color.White;
	public override float Layer { get; set; }

	public MText(string text, SpriteFont font)
	{
		Text = text;
		Font = font;
	}
	
	public override Rectangle Hitbox
	{
		get
		{
			Point location = (Position - Origin * Scale).ToPoint();
			Point size = MFontHelper.TextSize(Font, Text).ToPoint() * Scale.ToPoint();

			return new Rectangle(location, size);
		}
	}
	
	public override Vector2 Origin => Centered ? MFontHelper.TextSize(Font, Text) / 2 : Vector2.Zero;
	
	public override void Update(GameTime gameTime) { }

	public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		spriteBatch.DrawString(Font, Text, Position, Color, Rotation, Origin, Scale, SpriteEffects.None, Layer);
	}

	public override void OnAddToNode(MNode parent) { }

	public override void OnRemoveFromNode(MNode parent) { }
}