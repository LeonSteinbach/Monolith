using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.assets;
using Monolith.input;

namespace Monolith.graphics;

public class MButton
{
	private readonly MStaticSprite defaultSprite, hoverSprite;
	private readonly string text;
	private readonly SpriteFont font;
	private readonly Color color;
	private readonly string hoverSound, clickSound;

	private bool isHovering, wasHovering;
	private bool isPressed;

	public event Action<MButton> OnPressed; 

	public MButton(MStaticSprite defaultSprite, MStaticSprite hoverSprite, string hoverSound = null, 
		string clickSound = null, string text = null, SpriteFont font = null, Color color = default)
	{
		this.defaultSprite = defaultSprite ?? throw new ArgumentNullException(nameof(defaultSprite));
		this.hoverSprite = hoverSprite ?? throw new ArgumentNullException(nameof(hoverSprite));
		this.hoverSound = hoverSound;
		this.clickSound = clickSound;
		this.text = text;
		this.font = font;
		this.color = color;
	}
	
	public Rectangle Rectangle => Hover() ? hoverSprite.Hitbox : defaultSprite.Hitbox;
	
	public bool Hover() => isHovering;

	public bool Entered() => isHovering && !wasHovering;

	public bool Left() => !isHovering && wasHovering;

	public bool Pressed() => isPressed;
	
	public void Update()
	{
		wasHovering = isHovering;
		isHovering = Rectangle.Contains(MInput.MousePosition());

		isPressed = isHovering && MInput.IsLeftPressed();

		if (Entered() && hoverSound != null)
		{
			MAudioManager.PlaySound(hoverSound);
		}

		if (Pressed())
		{
			if (clickSound != null)
				MAudioManager.PlaySound(clickSound);
			
			OnPressed?.Invoke(this);
		}
	}

	public void Render(GameTime gameTime, SpriteBatch spriteBatch)
	{
		MStaticSprite sprite = Hover() ? hoverSprite : defaultSprite;

		sprite.Render(gameTime, spriteBatch);

		if (!string.IsNullOrEmpty(text) && font != null)
			spriteBatch.DrawString(font, text, sprite.Position, color, sprite.Rotation,
				sprite.Origin, sprite.Scale, SpriteEffects.None, sprite.Layer);
	}
}