using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monolith.assets;
using Monolith.input;
using Monolith.scene;

namespace Monolith.graphics;

public class MTextbox : MNode
{
	private bool isHovering, wasHovering;
	private bool isPressed;

	public event Action<MTextbox> OnSelected, OnDeselected, OnMouseHover, OnMouseEntered, OnMouseLeft, OnMousePressed; 
	
	public int MaxLength { get; set; }
	public bool Selected { get; set; }

	public int CursorOffset { get; set; }
	
	public MTextbox(MStaticSprite background, MText text, MAnimatedSprite cursor, MText placeholder = null, int maxLength = Int32.MaxValue, bool selected = false)
	{
		background.Name = "background";
		text.Name = "text";
		cursor.Name = "cursor";

		AddNode(background);
		AddNode(text);
		AddNode(cursor);
		
		if (placeholder != null)
		{
			placeholder.Name = "placeholder";
			AddNode(placeholder);
		}

		MaxLength = maxLength;
		Selected = selected;

		CursorOffset = (int)(MFontHelper.TextSize(Text.Font, " ").X * Text.Scale.X);
	}

	private MStaticSprite Background => GetNode<MStaticSprite>("background");

	public MText Text => GetNode<MText>("text");

	private MAnimatedSprite Cursor => GetNode<MAnimatedSprite>("cursor");

	private MText Placeholder => GetNode<MText>("placeholder");

	public override Rectangle Hitbox => Background.Hitbox;
	
	public bool MouseHover() => isHovering;

	public bool MouseEntered() => isHovering && !wasHovering;

	public bool MouseLeft() => !isHovering && wasHovering;

	public bool MousePressed() => isPressed;
	
	public override void Update(GameTime gameTime)
	{
		var key = MInput.GetPressedKey();
		
		if (key is >= Keys.A and <= Keys.Z && Text.Text.Length < MaxLength)
			Text.Text += Enum.Parse(typeof(Keys), key.ToString());

		if (key == Keys.Back && Text.Text.Length > 0)
			Text.Text = Text.Text.Substring(0, Text.Text.Length - 1);

		Cursor.Position = new Vector2(Text.Hitbox.Left + (Text.Text.Length > 0 ? Text.Hitbox.Width + CursorOffset : 0), Text.Position.Y);
		Cursor.Update(gameTime);
		
		wasHovering = isHovering;
		isHovering = Hitbox.Contains(MInput.MousePosition());

		isPressed = isHovering && MInput.IsLeftPressed();

		if (MouseHover())
			OnMouseHover?.Invoke(this);
		
		if (MouseEntered())
			OnMouseEntered?.Invoke(this);
		
		if (MouseLeft())
			OnMouseLeft?.Invoke(this);

		if (MousePressed())
		{
			Cursor.Start();
			Selected = true;
			OnSelected?.Invoke(this);
			OnMousePressed?.Invoke(this);
		}

		if (!MouseHover() && MInput.IsLeftPressed() && Selected)
		{
			Cursor.Stop();
			Selected = false;
			OnDeselected?.Invoke(this);
		}
	}

	public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		Background.Render(graphics, spriteBatch, gameTime);
		
		if (Text.Text.Length > 0)
			Text.Render(graphics, spriteBatch, gameTime);
		else
			Placeholder.Render(graphics, spriteBatch, gameTime);
		
		Cursor.Render(graphics, spriteBatch, gameTime);
	}

	public override void OnAddToNode(MNode parent) { }

	public override void OnRemoveFromNode(MNode parent) { }
}