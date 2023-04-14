using Microsoft.Xna.Framework;

namespace Monolith;

public class MonolithWindow
{
	private readonly MonolithGame game;

	private Point size;
	public Point Size
	{
		get => size;
		set
		{
			size = value;
			game.graphics.PreferredBackBufferWidth = value.X;
			game.graphics.PreferredBackBufferHeight = value.Y;
			game.graphics.ApplyChanges();
		}
	}

	private Point position;
	public Point Position
	{
		get => position;
		set
		{
			position = value;
			game.Window.Position = value;
			game.graphics.ApplyChanges();
		}
	}

	private string title;
	public string Title
	{
		get => title;
		set
		{
			title = value;
			game.Window.Title = value;
			game.graphics.ApplyChanges();
		}
	}

	private bool isCentered;
	public bool IsCentered
	{
		get => isCentered;
		set
		{
			isCentered = value;
			if (value)
			{
				game.Window.Position = new Point(
					(game.graphics.GraphicsDevice.DisplayMode.Width - game.graphics.PreferredBackBufferWidth) / 2,
					(game.graphics.GraphicsDevice.DisplayMode.Height - game.graphics.PreferredBackBufferHeight) / 2
				);
				game.graphics.ApplyChanges();
			}
		}
	}

	private bool isFixedTimeStep;
	public bool IsFixedTimeStep
	{
		get => isFixedTimeStep;
		set
		{
			isFixedTimeStep = value;
			game.IsFixedTimeStep = value;
			game.graphics.ApplyChanges();
		}
	}

	private bool isVSyncEnabled;
	public bool IsVSyncEnabled
	{
		get => isVSyncEnabled;
		set
		{
			isVSyncEnabled = value;
			game.graphics.SynchronizeWithVerticalRetrace = value;
			game.graphics.ApplyChanges();
		}
	}

	private bool isFullScreen;
	public bool IsFullScreen
	{
		get => isFullScreen;
		set
		{
			isFullScreen = value;
			if (value)
			{
				Size = new Point(
					game.GraphicsDevice.Adapter.CurrentDisplayMode.Width,
					game.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
				game.graphics.IsFullScreen = true;
				game.graphics.ApplyChanges();
			}
			else
			{
				game.graphics.IsFullScreen = false;
				game.graphics.ApplyChanges();
			}
		}
	}

	private bool isMouseVisible;
	public bool IsMouseVisible
	{
		get => isMouseVisible;
		set
		{
			isMouseVisible = value;
			game.IsMouseVisible = value;
			game.graphics.ApplyChanges();
		}
	}

	private bool isBorderless;
	public bool IsBorderless
	{
		get => isBorderless;
		set
		{
			isBorderless = value;
			if (value)
			{
				game.Window.IsBorderless = true;
				game.Window.Position = new Point(0, 0);
				game.graphics.PreferredBackBufferWidth = game.graphics.GraphicsDevice.DisplayMode.Width;
				game.graphics.PreferredBackBufferHeight = game.graphics.GraphicsDevice.DisplayMode.Height;
				game.graphics.ApplyChanges();
			}
			else
			{
				game.Window.IsBorderless = false;
				game.Window.Position = position;
				game.graphics.PreferredBackBufferWidth = size.X;
				game.graphics.PreferredBackBufferHeight = size.Y;
				game.graphics.ApplyChanges();
			}
		}
	}

	public MonolithWindow(MonolithGame game)
	{
		this.game = game;
	}
}