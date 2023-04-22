using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.graphics;
using Monolith.scene;

namespace Monolith.particles;

public class MParticle : MNode
{
	private readonly MSprite sprite;
	private Vector2 velocity;
	private readonly DateTime createdTime;
	private readonly float timeToLive;
	private float speed;
	private readonly float speedDelta;
	private readonly float rotationDelta;
	private readonly float scaleDelta;
	private float opacity;
	private readonly float opacityDelta;

	public MParticle(MSprite sprite, Vector2 velocity, float timeToLive)
	{
		this.sprite = sprite;
		this.velocity = velocity;
		this.timeToLive = timeToLive;

		createdTime = DateTime.Now;
		speed = 1f;
		speedDelta = 1f;
		rotationDelta = 1f;
		scaleDelta = 1f;
		opacity = 1f;
		opacityDelta = 1f;
	}

	public MParticle(MSprite sprite, Vector2 velocity, float timeToLive, 
		float speed, float speedDelta, float rotationDelta, float scaleDelta, float opacity, float opacityDelta)
	{
		this.sprite = sprite;
		this.velocity = velocity;
		this.timeToLive = timeToLive;

		createdTime = DateTime.Now;
		this.speed = speed;
		this.speedDelta = speedDelta;
		this.rotationDelta = rotationDelta;
		this.scaleDelta = scaleDelta;
		this.opacity = opacity;
		this.opacityDelta = opacityDelta;
	}

	private float Age => (float) (DateTime.Now - createdTime).TotalMilliseconds;

	public bool ShouldBeRemoved => Age >= timeToLive || sprite.Scale.X <= 0 || sprite.Scale.Y <= 0 || opacity <= 0;

	public override Rectangle Hitbox => sprite.Hitbox;

	public override void Update(GameTime gameTime)
	{
		speed += speedDelta * gameTime.ElapsedGameTime.Milliseconds;
		velocity *= speed * gameTime.ElapsedGameTime.Milliseconds;
		sprite.Position += velocity * gameTime.ElapsedGameTime.Milliseconds;
		sprite.Rotation += rotationDelta * gameTime.ElapsedGameTime.Milliseconds;
		sprite.Scale += new Vector2(scaleDelta) * gameTime.ElapsedGameTime.Milliseconds;
		opacity += opacityDelta * gameTime.ElapsedGameTime.Milliseconds;

		var color = sprite.Color;
		color.A = (byte) opacity;
		sprite.Color = color;
	}

	public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		sprite.Render(graphics, spriteBatch, gameTime);
	}

	public override void OnAddToNode(MNode parent) { }

	public override void OnRemoveFromNode(MNode parent) { }
}