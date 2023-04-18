using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.assets;
using Monolith.input;
using Monolith.scene;

namespace Monolith.graphics;

public class MButton : MNode
{
	private readonly string text;
	private readonly SpriteFont font;
	private readonly Color color;
	private readonly string hoverSound, clickSound;

	private bool isHovering, wasHovering;
	private bool isPressed;

	public event Action<MButton> OnMouseHover, OnMouseEntered, OnMouseLeft, OnMousePressed; 

	public MButton(MStaticSprite defaultSprite, MStaticSprite hoverSprite, string hoverSound = null, 
		string clickSound = null, string text = null, SpriteFont font = null, Color color = default)
	{
		defaultSprite.Name = "default";
		hoverSprite.Name = "hover";
		
		AddNode(defaultSprite);
		AddNode(hoverSprite);
		
		this.hoverSound = hoverSound;
		this.clickSound = clickSound;
		this.text = text;
		this.font = font;
		this.color = color;
	}

	public bool MouseHover() => isHovering;

	public bool MouseEntered() => isHovering && !wasHovering;

	public bool MouseLeft() => !isHovering && wasHovering;

	public bool MousePressed() => isPressed;

	public override Rectangle Hitbox => Sprite.Hitbox;

	private MSprite Sprite => MouseHover() ? GetNode<MStaticSprite>("hover") : GetNode<MStaticSprite>("default");

	public override void Update(GameTime gameTime)
	{
		wasHovering = isHovering;
		isHovering = Hitbox.Contains(MInput.MousePosition());

		isPressed = isHovering && MInput.IsLeftPressed();

		if (MouseHover())
			OnMouseHover?.Invoke(this);
		
		if (MouseEntered())
		{
			if (hoverSound != null)
				MAudioManager.PlaySound(hoverSound);
			OnMouseEntered?.Invoke(this);
		}
		
		if (MouseLeft())
			OnMouseLeft?.Invoke(this);

		if (MousePressed())
		{
			if (clickSound != null)
				MAudioManager.PlaySound(clickSound);
			OnMousePressed?.Invoke(this);
		}
	}

	public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		var sprite = Sprite;
		
		sprite.Render(graphics, spriteBatch, gameTime);

		if (!string.IsNullOrEmpty(text) && font != null)
			spriteBatch.DrawString(font, text, sprite.Position, color, sprite.Rotation,
				sprite.Origin, sprite.Scale, SpriteEffects.None, sprite.Layer);
	}

	public override void OnAddToNode(MNode parent) { }

	public override void OnRemoveFromNode(MNode parent) { }
}