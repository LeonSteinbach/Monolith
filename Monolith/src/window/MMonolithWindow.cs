using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monolith.window;

public class MMonolithWindow
{
	private readonly MMonolithGame game;

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
			game.Window.IsBorderless = isBorderless;
			game.graphics.ApplyChanges();
		}
	}
	
	private bool isResizeAllowed;
	public bool IsResizeAllowed
	{
		get => isResizeAllowed;
		set
		{
			isResizeAllowed = value;
			game.Window.AllowUserResizing = isResizeAllowed;
			game.graphics.ApplyChanges();
		}
	}

	public MMonolithWindow(MMonolithGame game)
	{
		this.game = game;
	}
}