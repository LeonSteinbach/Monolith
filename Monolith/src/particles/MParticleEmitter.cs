using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.graphics;

namespace Monolith.particles;

public class MParticleEmitter
{
	private DateTime timer;
	private readonly List<MParticle> particles;

	public MParticleEmitter()
	{
		timer = DateTime.Now;
		particles = new List<MParticle>();
	}

	public void Emit(int delay, MSprite sprite, Vector2 velocity, float timeToLive)
	{
		DateTime now = DateTime.Now;
		if ((now - timer).TotalMilliseconds >= delay) {
			timer = now;
			MParticle particle = new MParticle(sprite, velocity, timeToLive);
			particles.Add(particle);
		}
	}

	public void Emit(int delay, MSprite sprite, Vector2 velocity, float timeToLive, float speed, float speedDelta,
		float rotationDelta, float scaleDelta, float opacity, float opacityDelta)
	{
		DateTime now = DateTime.Now;
		if ((now - timer).TotalMilliseconds >= delay) {
			timer = now;
			MParticle particle = new MParticle(sprite, velocity, timeToLive, speed, 
				speedDelta, rotationDelta, scaleDelta, opacity, opacityDelta);
			particles.Add(particle);
		}
	}

	public void Update(GameTime gameTime)
	{
		for (int i = particles.Count - 1; i >= 0; i--) {
			particles[i].Update(gameTime);
			if (particles[i].ShouldBeRemoved)
				particles.Remove(particles[i]);
		}
	}

	public void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		for (int i = particles.Count - 1; i >= 0; i--) {
			particles[i].Render(graphics, spriteBatch, gameTime);
		}
	}
}