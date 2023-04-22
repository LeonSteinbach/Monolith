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
		var boundingBox = background.Hitbox.BoundingBox;
		
		int offset;
		if (direction == MSliderDirection.Horizontal)
		{
			offset = (int)MMathHelper.MapRange(x, MinValue, MaxValue, 0, boundingBox.Width);
			return new Vector2(boundingBox.Left + offset, boundingBox.Center.Y);
		}
		offset = (int)MMathHelper.MapRange(x, MinValue, MaxValue, 0, boundingBox.Height);
		return new Vector2(boundingBox.Center.X, boundingBox.Bottom - offset);
	}
	
	private int GetValueFromPosition()
	{
		var background = Background;
		var boundingBox = background.Hitbox.BoundingBox;
		var button = Button;
		
		if (direction == MSliderDirection.Horizontal)
			return (int)MMathHelper.MapRange(button.Position.X, boundingBox.Left, boundingBox.Right, MinValue, MaxValue);
		return (int)MMathHelper.MapRange(button.Position.Y, boundingBox.Bottom, boundingBox.Top, MinValue, MaxValue);
	}

	public MStaticSprite Background => GetNode<MStaticSprite>("background");

	public MButton Button => GetNode<MButton>("button");

	public override MPolygon Hitbox => Background.Hitbox;
	
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
		var boundingBox = background.Hitbox.BoundingBox;
		var button = Button;
		
		wasHovering = isHovering;
		isHovering = Hitbox.Intersect(MInput.MousePosition());

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
				if (newPosition < boundingBox.Left)
					button.Position = new Vector2(boundingBox.Left, button.Position.Y);
				else if (newPosition > boundingBox.Left + boundingBox.Width)
					button.Position = new Vector2(boundingBox.Left + boundingBox.Width, button.Position.Y);
				else
					button.Position = new Vector2(newPosition, button.Position.Y);
			}
			else
			{
				int newPosition = MInput.MousePosition().Y;
				if (newPosition < boundingBox.Top)
					button.Position = new Vector2(button.Position.X, boundingBox.Top);
				else if (newPosition > boundingBox.Top + boundingBox.Height)
					button.Position = new Vector2(button.Position.X, boundingBox.Top + boundingBox.Height);
				else
					button.Position = new Vector2(button.Position.X, newPosition);
			}

			Value = GetValueFromPosition();
		}
		
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