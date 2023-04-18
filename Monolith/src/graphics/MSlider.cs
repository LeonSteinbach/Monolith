using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.assets;
using Monolith.input;
using Monolith.math;
using Monolith.scene;

namespace Monolith.graphics;

public enum MSliderDirection
{
	Vertical,
	Horizontal
}

public class MSlider : MNode
{
	private readonly MSliderDirection direction;
	private readonly int steps;
	
	private int value;
	
	private bool isHovering, wasHovering;
	private bool isPressed, isButtonHold;

	public int MinValue { get; }
	public int MaxValue { get; }

	public event Action<MSlider> OnValueChanged, OnMouseHover, OnMouseEntered, OnMouseLeft, OnMousePressed;

	public MSlider(MStaticSprite background, MButton button, int minValue, int maxValue, int startValue,
		MSliderDirection direction, int steps = 0)
	{
		background.Name = "background";
		button.Name = "button";
		
		AddNode(background);
		AddNode(button);
		
		MinValue = minValue;
		MaxValue = maxValue;
		
		this.direction = direction;
		this.steps = steps;

		button.Position = GetPositionFromValue(startValue);
	}

	private Vector2 GetPositionFromValue(int x)
	{
		var background = Background;
		
		int offset;
		if (direction == MSliderDirection.Horizontal)
		{
			offset = (int)MMathHelper.MapRange(x, MinValue, MaxValue, 0, background.Hitbox.Width);
			return new Vector2(background.Hitbox.Left + offset, background.Hitbox.Center.Y);
		}
		offset = (int)MMathHelper.MapRange(x, MinValue, MaxValue, 0, background.Hitbox.Height);
		return new Vector2(background.Hitbox.Center.X, background.Hitbox.Bottom - offset);
	}
	
	private int GetValueFromPosition()
	{
		var background = Background;
		var button = Button;
		
		if (direction == MSliderDirection.Horizontal)
			return (int)MMathHelper.MapRange(button.Position.X, background.Hitbox.Left, background.Hitbox.Right, MinValue, MaxValue);
		return (int)MMathHelper.MapRange(button.Position.Y, background.Hitbox.Bottom, background.Hitbox.Top, MinValue, MaxValue);
	}

	public MStaticSprite Background => GetNode<MStaticSprite>("background");

	public MButton Button => GetNode<MButton>("button");

	public override Rectangle Hitbox => Background.Hitbox;
	
	public bool MouseHover() => isHovering;

	public bool MouseEntered() => isHovering && !wasHovering;

	public bool MouseLeft() => !isHovering && wasHovering;

	public bool MousePressed() => isPressed;

	public int Value
	{
		get => value;
		set
		{
			this.value = value;
			OnValueChanged?.Invoke(this);
		}
	}

	public override void Update(GameTime gameTime)
	{
		var background = Background;
		var button = Button;
		
		wasHovering = isHovering;
		isHovering = Hitbox.Contains(MInput.MousePosition());

		isPressed = isPressed || isHovering && MInput.IsLeftPressed();

		if (button.MousePressed())
			isButtonHold = true;
		else if (MInput.IsLeftReleased())
			isButtonHold = false;
		
		if (MouseHover())
			OnMouseHover?.Invoke(this);
		
		if (MouseEntered())
			OnMouseEntered?.Invoke(this);

		if (MouseLeft())
			OnMouseLeft?.Invoke(this);

		if (MousePressed())
			OnMousePressed?.Invoke(this);

		if (isButtonHold)
		{
			if (direction == MSliderDirection.Horizontal)
			{
				int newPosition = MInput.MousePosition().X;
				if (newPosition < background.Hitbox.Left)
					button.Position = new Vector2(background.Hitbox.Left, button.Position.Y);
				else if (newPosition > background.Hitbox.Left + background.Hitbox.Width)
					button.Position = new Vector2(background.Hitbox.Left + background.Hitbox.Width, button.Position.Y);
				else
					button.Position = new Vector2(newPosition, button.Position.Y);
			}
			else
			{
				int newPosition = MInput.MousePosition().Y;
				if (newPosition < background.Hitbox.Top)
					button.Position = new Vector2(button.Position.X, background.Hitbox.Top);
				else if (newPosition > background.Hitbox.Top + background.Hitbox.Height)
					button.Position = new Vector2(button.Position.X, background.Hitbox.Top + background.Hitbox.Height);
				else
					button.Position = new Vector2(button.Position.X, newPosition);
			}

			Value = GetValueFromPosition();
		}
		
		UpdateChildren(gameTime);
	}

	public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		RenderChildren(graphics, spriteBatch, gameTime);
	}

	public override void OnAddToNode(MNode parent) { }

	public override void OnRemoveFromNode(MNode parent) { }
}