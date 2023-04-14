using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monolith.particles;

public class MParticle
{
	private readonly Texture2D texture;
	private Color color;
	private Vector2 position, velocity;
	private readonly DateTime createdTime;
	private readonly float timeToLive;
	private float speed;
	private readonly float speedDelta;
	private float angle;
	private readonly float angleDelta;
	private float scale;
	private readonly float scaleDelta;
	private float opacity;
	private readonly float opacityDelta;
	private readonly float layer;

	public MParticle(Texture2D texture, Vector2 position, Vector2 velocity, float timeToLive)
	{
		this.texture = texture;
		this.position = position;
		this.velocity = velocity;
		this.timeToLive = timeToLive;

		createdTime = DateTime.Now;
		color = Color.White;
		speed = 1f;
		speedDelta = 1f;
		angle = 0f;
		angleDelta = 1f;
		scale = 1f;
		scaleDelta = 1f;
		opacity = 1f;
		opacityDelta = 1f;
		layer = 1f;
	}

	public MParticle(Texture2D texture, Vector2 position, Vector2 velocity, float timeToLive, 
		Color color, float speed, float speedDelta, float angle, float angleDelta, 
		float scale, float scaleDelta, float opacity, float opacityDelta, float layer)
	{
		this.texture = texture;
		this.position = position;
		this.velocity = velocity;
		this.timeToLive = timeToLive;

		createdTime = DateTime.Now;
		this.color = color;
		this.speed = speed;
		this.speedDelta = speedDelta;
		this.angle = angle;
		this.angleDelta = angleDelta;
		this.scale = scale;
		this.scaleDelta = scaleDelta;
		this.opacity = opacity;
		this.opacityDelta = opacityDelta;
		this.layer = layer;
	}

	private float Age => (float) (DateTime.Now - createdTime).TotalMilliseconds;

	public bool ShouldBeRemoved => Age >= timeToLive || scale <= 0 || opacity <= 0;

	public void Update(GameTime gameTime)
	{
		speed += speedDelta * gameTime.ElapsedGameTime.Milliseconds;
		velocity *= speed * gameTime.ElapsedGameTime.Milliseconds;
		position += velocity * gameTime.ElapsedGameTime.Milliseconds;
		angle += angleDelta * gameTime.ElapsedGameTime.Milliseconds;
		scale += scaleDelta * gameTime.ElapsedGameTime.Milliseconds;
		opacity += opacityDelta * gameTime.ElapsedGameTime.Milliseconds;

		color.A = (byte) opacity;
	}

	public void Render(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(texture, position, null, color, angle, texture.Bounds.Center.ToVector2(), scale, SpriteEffects.None, layer);
	}
}