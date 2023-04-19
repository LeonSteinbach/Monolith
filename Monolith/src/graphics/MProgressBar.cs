using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.input;
using Monolith.scene;

namespace Monolith.graphics;

public enum MProgressBarDirection
{
	Horizontal,
	Vertical
}

public class MProgressBar : MNode
{
	private bool isHovering, wasHovering;
	private bool isPressed;

	private float value;

	public event Action<MProgressBar> OnValueChanged, OnFinished, OnMouseHover, OnMouseEntered, OnMouseLeft, OnMousePressed; 
	
	public float MinValue { get; set; }
	
	public float MaxValue { get; set; }
	
	public MProgressBarDirection Direction { get; set; }

	public float Value
	{
		get => value;
		set
		{
			if (value < MinValue)
				value = MinValue;
			if (value > MaxValue)
				value = MaxValue;
			
			this.value = value;
			OnValueChanged?.Invoke(this);
			if (value >= MaxValue)
				OnFinished?.Invoke(this);
		}
	}

	public float ValueDecimal => Value / MaxValue;
	
	public MProgressBar(MStaticSprite background, MStaticSprite foreground, float minValue, float maxValue, float startValue,
		MText text = null, MProgressBarDirection direction = MProgressBarDirection.Horizontal)
	{
		background.Name = "background";
		foreground.Name = "foreground";
		
		AddNode(background);
		AddNode(foreground);

		if (text != null)
		{
			text.Name = "text";
			AddNode(text);
		}

		MinValue = minValue;
		MaxValue = maxValue;
		Value = startValue;
		Direction = direction;
	}

	private MStaticSprite Background => GetNode<MStaticSprite>("background");

	private MStaticSprite Foreground => GetNode<MStaticSprite>("foreground");

	public MText Text => GetNode<MText>("text");

	public override Rectangle Hitbox => Background.Hitbox;
	
	public bool MouseHover() => isHovering;

	public bool MouseEntered() => isHovering && !wasHovering;

	public bool MouseLeft() => !isHovering && wasHovering;

	public bool MousePressed() => isPressed;

	private Rectangle GetSourceOffset()
	{
		var hitbox = new Rectangle(
			Foreground.Hitbox.Left,
			Foreground.Hitbox.Right,
			(int)(Foreground.Hitbox.Width / Foreground.Scale.X),
			(int)(Foreground.Hitbox.Height / Foreground.Scale.Y));

		return Direction switch
		{
			MProgressBarDirection.Vertical =>
				new Rectangle(0, 0, hitbox.Width, (int) (hitbox.Height * ValueDecimal)),
			MProgressBarDirection.Horizontal =>
				new Rectangle(0, 0, (int) (hitbox.Width * ValueDecimal), hitbox.Height),
			_ => throw new ArgumentException("Invalid direction")
		};
	}
	
	public override void Update(GameTime gameTime)
	{
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
			OnMousePressed?.Invoke(this);

		Foreground.SourceOffset = GetSourceOffset();
	}

	public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		if (!IsVisible) return;
		
		Background.Render(graphics, spriteBatch, gameTime);
		Foreground.Render(graphics, spriteBatch, gameTime);
		Text?.Render(graphics, spriteBatch, gameTime);
	}

	public override void OnAddToNode(MNode parent) { }

	public override void OnRemoveFromNode(MNode parent) { }
}