using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monolith.graphics;

public class MRadioGroup
{
	private readonly List<MRadioButton> radioButtons = new ();
	private MRadioButton selectedButton;

	public void AddButton(MRadioButton button)
	{
		radioButtons.Add(button);

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
		foreach (MRadioButton otherButton in radioButtons)
		{
			if (otherButton != button)
			{
				otherButton.Deselect();
			}
		}

		selectedButton = button;
	}

	public string GetSelectedText()
	{
		return selectedButton?.Text;
	}

	public void Update()
	{
		foreach (MRadioButton button in radioButtons)
		{
			button.Update();
		}
	}

	public void Render(GameTime gameTime, SpriteBatch spriteBatch)
	{
		foreach (MRadioButton button in radioButtons)
		{
			button.Render(gameTime, spriteBatch);
		}
	}
}