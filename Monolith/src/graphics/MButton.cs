using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.assets;
using Monolith.input;
using Monolith.scene;

namespace Monolith.graphics;

public class MButton : MNode
{
	private readonly string hoverSound, clickSound;

	private bool isHovering, wasHovering;
	private bool isPressed;

	public event Action<MButton> OnMouseHover, OnMouseEntered, OnMouseLeft, OnMousePressed; 

	public MButton(MStaticSprite defaultSprite, MStaticSprite hoverSprite, MText text = null, string hoverSound = null, 
		string clickSound = null, SpriteFont font = null, Color color = default)
	{
		defaultSprite.Name = "default";
		hoverSprite.Name = "hover";
		
		AddNode(defaultSprite);
		AddNode(hoverSprite);
		
		if (text != null)
		{
			text.Name = "text";
			AddNode(text);
		}
		
		this.hoverSound = hoverSound;
		this.clickSound = clickSound;
	}

	public bool MouseHover() => isHovering;

	public bool MouseEntered() => isHovering && !wasHovering;

	public bool MouseLeft() => !isHovering && wasHovering;

	public bool MousePressed() => isPressed;

	public override Rectangle Hitbox => Sprite.Hitbox;

	private MSprite Sprite => MouseHover() ? GetNode<MStaticSprite>("hover") : GetNode<MStaticSprite>("default");

	public MText Text => GetNode<MText>("text");

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
		Sprite.Render(graphics, spriteBatch, gameTime);
		Text?.Render(graphics, spriteBatch, gameTime);
	}

	public override void OnAddToNode(MNode parent) { }

	public override void OnRemoveFromNode(MNode parent) { }
}