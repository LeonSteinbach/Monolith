using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.graphics;
using Monolith.math;
using Monolith.scene;

namespace Monolith.physics;

public class MBody : MNode
{
	public float Inertia { get; set; } = 1f;
	public float InverseInertia => Inertia > 0 ? 1 / Inertia : 0f;
	public Vector2 LinearVelocity { get; set; } = Vector2.Zero;
	public float RotationalVelocity { get; set; } = 0f;
	public float Density { get; set; } = 1f;
	public float Mass => Hitbox.Area * Density;
	public float InverseMass => Mass > 0 ? 1 / Mass : 0f;
	public float Restitution { get; set; } = 0.1f;
	public float StaticFriction { get; set; } = 0.0f;
	public float DynamicFriction { get; set; } = 0.0f;
	private Vector2 force;
	public Vector2 Gravity { get; set; } = new (0f);
	public bool IsStatic { get; set; } = false;
	public Color DebugColor { get; set; } = Color.White;

	public override MGeometryObject Hitbox { get; }

	public override void Update(GameTime gameTime)
	{
		if (IsStatic)
			return;

		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

		LinearVelocity += Gravity * dt;
		Position += LinearVelocity * dt;
		Rotation += RotationalVelocity * dt;
		
		force = Vector2.Zero;
	}

	public override void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		switch (Hitbox)
		{
			case MPolygon polygon:
				polygon.Render(spriteBatch, DebugColor, true, false, true);
				break;
			case MCircle circle:
				circle.Render(spriteBatch, DebugColor, true, false, true);
				break;
		}

		var dir = LinearVelocity;
		dir.Normalize();
		dir *= 50;
		MShapes.DrawLine(spriteBatch, Hitbox.CenterOfMass, Hitbox.CenterOfMass + dir, Color.Red, 2f);
	}

	public void Move(Vector2 vector)
	{
		Position += vector;
		Hitbox.Move(vector);
	}

	public void MoveTo(Vector2 position)
	{
		Position = position;
		Hitbox.Move(position - Hitbox.CenterOfMass);
	}

	public void Rotate(float angle)
	{
		Rotation += angle;
		Hitbox.Rotate(angle);
	}

	public void AddForce(Vector2 vector)
	{
		force = vector;
	}
	
	public override void OnAddToNode(MNode parent) { }

	public override void OnRemoveFromNode(MNode parent) { }
}