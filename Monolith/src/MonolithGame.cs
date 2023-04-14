using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monolith.assets;
using Monolith.diagnostics;
using Monolith.input;

namespace Monolith;

public class MonolithGame : Game
{
	public GraphicsDeviceManager graphics;
	protected SpriteBatch spriteBatch;

	protected MonolithGame()
	{
		graphics = new GraphicsDeviceManager(this);
	}

	protected override void Initialize()
	{
		AssetManager.Initialize();
		AudioManager.Initialize();
		
		base.Initialize();
	}

	protected override void LoadContent()
	{
		spriteBatch = new SpriteBatch(GraphicsDevice);
	}

	protected override void Update(GameTime gameTime)
	{
		Input.Update();
		if (Input.IsKeyPressed(Keys.Escape))
			Exit();
		
		TimeHelper.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		base.Draw(gameTime);
	}
}