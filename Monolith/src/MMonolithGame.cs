using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monolith.assets;
using Monolith.diagnostics;
using Monolith.input;

namespace Monolith;

public class MMonolithGame : Game
{
	public GraphicsDeviceManager graphics;
	protected SpriteBatch spriteBatch;

	protected MMonolithGame()
	{
		graphics = new GraphicsDeviceManager(this);
	}

	protected override void Initialize()
	{
		MAssetManager.Initialize();
		MAudioManager.Initialize();
		
		base.Initialize();
	}

	protected override void LoadContent()
	{
		spriteBatch = new SpriteBatch(GraphicsDevice);
	}

	protected override void Update(GameTime gameTime)
	{
		MInput.Update();
		if (MInput.IsKeyPressed(Keys.Escape))
			Exit();
		
		MTimeHelper.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		base.Draw(gameTime);
	}
}