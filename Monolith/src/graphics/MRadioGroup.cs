using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.scene;

namespace Monolith.graphics;

public class MRadioGroup : MNode
{
	private MRadioButton selectedButton;

	public void AddButton(MRadioButton button)
	{
		AddNode(button);

		if (button.IsSelected())
		{
			if (selectedButton != null)
				throw new Exception("Only one radio button can be selected!");
			selectedButton = button;
		}
		
		button.OnSelected += RadioButtonSelected;
	}

	private void RadioButtonSelected(MRadioButton button)
	{
		var a = GetAllNodes<MNode>();
		
		foreach (MRadioButton otherButton in GetAllNodes<MRadioButton>())
		{
			if (otherButton != button)
			{
				otherButton.Deselect();
			}
		}

		selectedButton = button;
	}

	public string SelectedValue => selectedButton?.Value;
	
	public string SelectedText => selectedButton?.Text.Text;

	public override Rectangle Hitbox => Rectangle.Empty;

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